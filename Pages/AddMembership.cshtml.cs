using AFRA.Members.Backend.database;
using AFRA.Members.Backend.database.Tables;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFRA.Members.Pages;

public class AddMembership : PageModel {
	public void OnPost() {
		using var db = new Database.DbConn();
		var       memberId = int.Parse(RouteData.Values["memberId"]!.ToString()!);
		using var transaction = db.BeginTransaction();

		try {
			var member   = db.Members.First(p => p.MemberId == memberId);
			db.Insert(new Membership
			{
				MemberId = memberId,
				Start = DateTime.Parse(Request.Form["membership_start"]!), // Assuming it's required
				End = string.IsNullOrEmpty(Request.Form["membership_end"]) ? null : DateTime.Parse(Request.Form["membership_end"]!),
				MonthlyFeeCents = string.IsNullOrEmpty(Request.Form["fee"]) ? null : int.Parse(Request.Form["fee"]!),
				Notes = Request.Form["notes"],
			});

			// Commit transaction if everything is successful
			transaction.Commit();

			var query = string.IsNullOrWhiteSpace(member.Nick) ? $"{member.FirstName} {member.LastName}" : member.Nick;
		
			Response.Redirect($"/#member_{query}");
		} catch {
			// Rollback transaction in case of an error
			transaction.Rollback();
            throw;
        }

	}
}
