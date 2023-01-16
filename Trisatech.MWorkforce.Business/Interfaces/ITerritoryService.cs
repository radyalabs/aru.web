using Trisatech.MWorkforce.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Interfaces
{
	public interface ITerritoryService
	{
		List<TerritoryModel> Get(string keyword = "", int limit = 20, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc");
		void Add(TerritoryModel model, string createdBy);
		void Edit(TerritoryModel model, string updatedBy);
		void Delete(string id, string deletedBy);
		TerritoryModel Get(string id);
		int Total(string search = "", int length = 10, int start = 0, string v = "CreatedDt", string orderType = "desc");
		//void Add2(TerritoryModel territory, string userId);
	}
}