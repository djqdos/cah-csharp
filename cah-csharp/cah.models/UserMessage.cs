using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace cah.models
{
	public class UserMessage
	{
		public string UserName { get; set; }

		public string Message { get; set; }

		public bool CurrentUser { get; set; }

		public DateTime DateSent { get; set; }	
	}
}
