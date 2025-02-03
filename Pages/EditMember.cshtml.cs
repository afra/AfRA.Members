using AFRA.Members.Backend.database;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFRA.Members.Pages;

public class EditMember : PageModel {
	public void OnGet() {
		// using var db       = new Database.DbConn();
		// var       memberId = int.Parse(RouteData.Values["id"]!.ToString()!);
		// var       member   = db.Members.First(p => p.MemberId == memberId);
	}

	public void OnPost() {
		using var db       = new Database.DbConn();
		var       memberId = int.Parse(RouteData.Values["id"]!.ToString()!);
		var       member   = db.Members.First(p => p.MemberId == memberId);

		if (Request.Form["action"] == "delete") {
			db.Delete(member);
			Response.Redirect("/");
			return;
		}

		member.FirstName           = Request.Form["first_name"];
		member.LastName            = Request.Form["last_name"];
		member.Nick                = Request.Form["nick"];
		member.Street              = Request.Form["street"];
		member.Zip                 = Request.Form["zip"];
		member.City                = Request.Form["city"];
		member.Country             = Request.Form["country"];
		member.FirstAddressLine    = Request.Form["first_address_line"];
		member.DateOfBirth         = string.IsNullOrEmpty(Request.Form["dob"]) ? null : DateTime.Parse(Request.Form["dob"]);
		member.Email               = Request.Form["email"];
		member.GpgKeyId            = Request.Form["gpg"];
		member.Phone               = Request.Form["phone"];
		member.Language            = Request.Form["language"];
		member.AccountingQuery     = Request.Form["accounting_query"];
		member.Iban                = Request.Form["iban"];
		member.Bic                 = Request.Form["bic"];
		member.MembershipStart     = string.IsNullOrEmpty(Request.Form["membership_start"]) ? null : DateTime.Parse(Request.Form["membership_start"]);
		member.MembershipEnd       = string.IsNullOrEmpty(Request.Form["membership_end"]) ? null : DateTime.Parse(Request.Form["membership_end"]);
		member.Founding            = Request.Form["founding"] == "on";
		member.TypeOfPayment       = Request.Form["type_of_payment"];
		member.SendDonationReceipt = Request.Form["send_donation_receipt"] == "on";
		member.MonthlyFeeCents     = string.IsNullOrEmpty(Request.Form["fee"]) ? null : int.Parse(Request.Form["fee"]);
		member.Notes               = Request.Form["notes"];
		member.NonVoting           = Request.Form["non_voting"] == "on";
		member.UpdatedAt           = DateTime.Now;

		db.Update(member);

		var query = string.IsNullOrWhiteSpace(member.Nick) ? $"{member.FirstName} {member.LastName}" : member.Nick;
		
		Response.Redirect($"/#member_{query}");
	}
}
