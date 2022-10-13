using AFRA.Members.database;
using AFRA.Members.database.Tables;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFRA.Members.Pages;

public class AddMember : PageModel {
	public static void OnGet() { }

	public void OnPost() {
		using var db = new Database.DbConn();
		var member = new Member {
			FirstName           = Request.Form["first_name"],
			LastName            = Request.Form["last_name"],
			Nick                = Request.Form["nick"],
			Street              = Request.Form["street"],
			Zip                 = Request.Form["zip"],
			City                = Request.Form["city"],
			Country             = Request.Form["country"],
			FirstAddressLine    = Request.Form["first_address_line"],
			DateOfBirth         = string.IsNullOrEmpty(Request.Form["dob"]) ? null : DateTime.Parse(Request.Form["dob"]),
			Email               = Request.Form["email"],
			GpgKeyId            = Request.Form["gpg"],
			Phone               = Request.Form["phone"],
			Language            = Request.Form["language"],
			AccountingQuery     = Request.Form["accounting_query"],
			Iban                = Request.Form["iban"],
			Bic                 = Request.Form["bic"],
			MembershipStart     = string.IsNullOrEmpty(Request.Form["membership_start"]) ? null : DateTime.Parse(Request.Form["membership_start"]),
			MembershipEnd       = string.IsNullOrEmpty(Request.Form["membership_end"]) ? null : DateTime.Parse(Request.Form["membership_end"]),
			Founding            = Request.Form["founding"] == "on",
			TypeOfPayment       = Request.Form["type_of_payment"],
			SendDonationReceipt = Request.Form["send_donation_receipt"] == "on",
			MonthlyFeeCents     = string.IsNullOrEmpty(Request.Form["fee"]) ? null : int.Parse(Request.Form["fee"]),
			Notes               = Request.Form["notes"],
			NonVoting           = Request.Form["non_voting"] == "on"
		};

		db.InsertWithIdentity(member);

		Response.Redirect(Request.Form["another"] == "true" ? "/AddMember" : "/");
	}
}
