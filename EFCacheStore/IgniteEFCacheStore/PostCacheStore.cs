using Apache.Ignite.Core.Cache.Store;
using Apache.Ignite.Core.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace IgniteEFCacheStore
{
   /// <summary>
   /// Ignite Cache Store for <see cref="Post"/> entities.
   /// </summary>
   public class PostCacheStore : ICacheStore<int, Post>
   {
      public void LoadCache(Action<int, Post> act, params object[] args)
      {
         Console.WriteLine("{0}.LoadCache() called.", GetType().Name);

         // Load everything from DB to Ignite
         using (var ctx = GetDbContext())
         {
            foreach (var post in ctx.Posts)
            {
               act(post.PostId, post);
            }
         }
      }

      public Post Load(int key)
      {
         Console.WriteLine("{0}.Load({1}) called.", GetType().Name, key);

         using (var ctx = GetDbContext())
         {
            return ctx.Posts.Find(key);
         }
      }

      public IEnumerable<KeyValuePair<int, Post>> LoadAll(IEnumerable<int> keys)
      {
         using (var ctx = GetDbContext())
         {
            return keys.Cast<int>().ToDictionary(key => key, key => ctx.Posts.Find(key));
         }
      }

      public void Write(int key, Post val)
      {
         Console.WriteLine("{0}.Write({1}, {2}) called.", GetType().Name, key, val);

         using (var ctx = GetDbContext())
         {
            ctx.Posts.AddOrUpdate((Post)val);

            ctx.SaveChanges();
         }
      }

      public void WriteAll(IEnumerable<KeyValuePair<int, Post>> entries)
      {
         using (var ctx = GetDbContext())
         {
            foreach (var post in entries)
            {
               ctx.Posts.AddOrUpdate(post.Value);

               ctx.SaveChanges();
            }
         }
      }

      public void Delete(int key)
      {
         Console.WriteLine("{0}.Delete({1}) called.", GetType().Name, key);

         using (var ctx = GetDbContext())
         {
            var post = ctx.Posts.Find(key);

            if (post != null)
            {
               ctx.Posts.Remove(post);

               ctx.SaveChanges();
            }
         }
      }

      public void DeleteAll(IEnumerable<int> keys)
      {
         foreach (var key in keys)
         {
            Delete(key);
         }
      }

      public void SessionEnd(bool commit)
      {
         // No-op.
      }

      private static BloggingContext GetDbContext()
      {
         return new BloggingContext
         {
            Configuration =
                {
                    // Disable EF proxies so that Ignite serialization works.
                    // https://apacheignite-net.readme.io/docs/serialization#using-entity-framework-pocos
                    ProxyCreationEnabled = false
                }
         };
      }
   }

   [Serializable]
   public class PostCacheStoreFactory : IFactory<ICacheStore>
   {
      public ICacheStore CreateInstance()
      {
         return new PostCacheStore();

         /*
         return new EntityFrameworkCacheStore<Post, BloggingContext>(
             () => new BloggingContext {Configuration = {ProxyCreationEnabled = false}},
             ctx => ctx.Posts,
             post => post.PostId,
             (post, key) =>
             {
                 post.PostId = (int) key;
             });

         */
      }
   }
}
