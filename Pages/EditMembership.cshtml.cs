using AFRA.Members.Backend.database;
using AFRA.Members.Backend.database.Tables;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFRA.Members.Pages;

public class EditMembership : PageModel {
	public void OnPost() {
		using var db = new Database.DbConn();
		var       membershipId = int.Parse(RouteData.Values["membershipId"]!.ToString()!);
		var       membership   = db.Memberships.First(p => p.MembershipId == membershipId);
		var       member = db.Members.First(member => member.MemberId == membership.MemberId);

		membership.Start = string.IsNullOrEmpty(Request.Form["membership_start"]) ? null : DateTime.Parse(Request.Form["membership_start"]);;
		membership.End = string.IsNullOrEmpty(Request.Form["membership_end"]) ? null : DateTime.Parse(Request.Form["membership_end"]);;
		membership.MonthlyFeeCents = string.IsNullOrEmpty(Request.Form["fee"]) ? null : int.Parse(Request.Form["fee"]!);
		membership.Notes = Request.Form["notes"];

		db.Update(membership);

		var query = string.IsNullOrWhiteSpace(member.Nick) ? $"{member.FirstName} {member.LastName}" : member.Nick;
	
		Response.Redirect($"/#member_{query}");
	}
}
