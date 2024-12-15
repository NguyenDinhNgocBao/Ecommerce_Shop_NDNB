﻿using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Shop_NDNB.Models
{
	public class BrandModel
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập tên Thương Hiệu")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập mô tả")]
		public string Description { get; set; }
		public int Status { get; set; }
		public string Slug { get; set; }
	}
}
