using AFRA.Members.Backend.database;
using AFRA.Members.Backend.database.Tables;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFRA.Members.Pages;

public class AddMember : PageModel {
	public static void OnGet() { }

	public void OnPost() {
		using var db = new Database.DbConn();
		using var transaction = db.BeginTransaction();

		try {
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
				MembershipStart     = string.IsNullOrEmpty(Request.Form["membership_start"]) ? null : DateTime.Parse(Request.Form["membership_start"]!),
				MembershipEnd       = string.IsNullOrEmpty(Request.Form["membership_end"]) ? null : DateTime.Parse(Request.Form["membership_end"]!),
				Founding            = Request.Form["founding"] == "on",
				TypeOfPayment       = Request.Form["type_of_payment"],
				SendDonationReceipt = Request.Form["send_donation_receipt"] == "on",
				MonthlyFeeCents     = string.IsNullOrEmpty(Request.Form["fee"]) ? null : int.Parse(Request.Form["fee"]!),
				Notes               = Request.Form["notes"],
				NonVoting           = Request.Form["non_voting"] == "on",
				CreatedAt           = DateTime.Now,
				UpdatedAt           = DateTime.Now,
			};
			var memberId = Convert.ToInt32(db.InsertWithIdentity(member));

			// Insert Membership linked to the Member
			db.Insert(new Membership
			{
				MemberId = memberId,
				Start = DateTime.Parse(Request.Form["membership_start"]!), // Assuming it's required
				End = string.IsNullOrEmpty(Request.Form["membership_end"]) ? null : DateTime.Parse(Request.Form["membership_end"]!),
				MonthlyFeeCents = string.IsNullOrEmpty(Request.Form["fee"]) ? null : int.Parse(Request.Form["fee"]!)
			});

			// Commit transaction if everything is successful
			transaction.Commit();

			var query = string.IsNullOrWhiteSpace(member.Nick) ? $"{member.FirstName} {member.LastName}" : member.Nick;
		
			Response.Redirect(Request.Form["another"] == "true" ? "/AddMember" : $"/#member_{query}");
		} catch {
			// Rollback transaction in case of an error
			transaction.Rollback();
            throw;
        }

	}
}
