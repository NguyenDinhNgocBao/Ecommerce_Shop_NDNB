using Newtonsoft.Json;
namespace Ecommerce_Shop_NDNB.Repository
{
	public static class SessionExtensions
	{
		//set gía trị cho key
		public static void SetJson(this ISession session, string key, object value) //lưu dữ liệu vào session dưới dạng JSON
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		} 
		//get json 
		public static T GetJson<T>(this ISession session, string key)//ấy dữ liệu từ session và chuyển đổi nó từ JSON thành đối tượng cụ thể
		{
			var sessionData = session.GetString(key);
			//nếu sessionData rỗng thì trả về mặc định T còn không thì sẽ convert key qua Json
			return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
		}
	}
}
