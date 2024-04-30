namespace Labb2ProgTemplate;

public class Membership : Customer
{
    public string MembershipTier { get; }

    public Membership(string name, string password, string membership) 
        : base (name, password)
    {
        MembershipTier = membership;
    }

    public string MembershipStatus()
    {
        string membershipColor = string.Empty;

        if (MembershipTier != null)
        {
            if (MembershipTier == "gold")
            {
                membershipColor = "\u001b[33m";
            }
            else if (MembershipTier == "silver")
            {
                membershipColor = "\u001b[34m";
            }
            else if (MembershipTier == "bronze")
            {
                membershipColor = "\x1b[31m";
            }
            else
            {
                membershipColor = "\u001b[37m";
            }
        }

        string resetColor = "\u001b[0m";

        return $"\nMEMBERSHIP:\t\t{membershipColor}{MembershipTier.ToUpper()}{resetColor}\n";
    }
}
