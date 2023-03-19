using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hawk.Models;

public class User
{
    [Required]
    public string Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; } = true;
    public string Language { get; set; } = "en_US";
}

public class UserInfo
{
    public string Country { get; set; }
    public string Sub { get; set; }
    public bool EmailVerified { get; set; }
    public object PlayerPlocale { get; set; }
    public long CountryAt { get; set; }
    public Pw Pw { get; set; }
    public bool PhoneNumberVerified { get; set; }
    public bool AccountVerified { get; set; }
    public object Ppid { get; set; }
    public string PlayerLocale { get; set; }
    public Acct Acct { get; set; }
    public int Age { get; set; }
    public string Jti { get; set; }
    public Affinity Affinity { get; set; }

    public Entitlement Entitlement { get; set; }
}

public class Pw
{
    public long CngAt { get; set; }
    public bool Reset { get; set; }
    public bool MustReset { get; set; }
}

public class Acct
{
    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("adm")]
    public bool Adm { get; set; }

    [JsonProperty("game_name")]
    public string GameName { get; set; }

    [JsonProperty("tag_line")]
    public string TagLine { get; set; }

    [JsonProperty("created_at")]
    public long CreatedAt { get; set; }
}

public class Affinity
{
    [JsonProperty("pp")]
    public string Region { get; set; }
}

public class Entitlement
{
    public string EntitlementsToken { get; set; }
}