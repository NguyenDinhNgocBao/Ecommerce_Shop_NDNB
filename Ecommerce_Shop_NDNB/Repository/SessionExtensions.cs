using Newtonsoft.Json;
namespace Ecommerce_Shop_NDNB.Repository
{
	public static class SessionExtensions
	{
		//set gía trị cho key
		public static void SetJson(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		} 
		//get json 
		public static T GetJson<T>(this ISession session, string key){
			var sessionData = session.GetString(key);
			//nếu sessionData rỗng thì trả về mặc định T còn không thì sẽ convert key qua Json
			return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
		}
	}
}
