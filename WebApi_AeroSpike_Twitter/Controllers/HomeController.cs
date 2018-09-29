using Aerospike.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi_AeroSpike_Twitter.Models;

namespace WebApi_AeroSpike_Twitter.Controllers
{
    public class HomeController : ApiController
    {
        // GET: Home

        string nameSpace = "AirEngine";
        string setName = "Vishwas";


        [HttpPost]
        public List<Fields> GetData([FromBody]List<long> tweet_id_list)
        {
            var aerospikeClient = new AerospikeClient("18.235.70.103", 3000);
            List<Fields> fieldData = new List<Fields>();
            foreach (long tid in tweet_id_list)
            {
                    Fields field = new Fields();
                    Record record = aerospikeClient.Get(new BatchPolicy(), new Key(nameSpace, setName, tid.ToString()));
                    field.author_id = record.GetValue("author_id").ToString();
                    field.author = record.GetValue("author").ToString();
                    field.content = record.GetValue("content").ToString();
                    field.region = record.GetValue("region").ToString();
                    field.language = record.GetValue("language").ToString();
                    field.following = Convert.ToInt32(record.GetValue("following").ToString());
                    field.followers = Convert.ToInt32(record.GetValue("followers").ToString());
                    field.tweet_date = record.GetValue("tweet_date").ToString();
                    field.post_type = record.GetValue("post_type").ToString();
                    field.post_url = record.GetValue("post_url").ToString();
                    field.retweet = record.GetValue("retweet").ToString();
                    field.tweet_id = record.GetValue("tweet_id").ToString();
                    fieldData.Add(field);
            }
            return fieldData;
        }

        [HttpPut]
        public void Update([FromBody]List<string> content)
        {
            var aerospikeClient = new AerospikeClient("18.235.70.103", 3000);
            string tweet_id = content[0];
            string binName = content[1];
            string newValue = content[2];
            aerospikeClient.Put(new WritePolicy(), new Key(nameSpace, setName,tweet_id), new Bin[] { new Bin(binName, newValue) });

        }


        [HttpDelete]
        public void Delete([FromBody]long tweet_id)
        {
            var aerospikeClient = new AerospikeClient("18.235.70.103", 3000);
            aerospikeClient.Delete(new WritePolicy(), new Key(nameSpace, setName, tweet_id));
        }
    }
}