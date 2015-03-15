using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	public class RandomNameGenerator
	{
		private Random _rnd;

		#region Имена набор 1

		public static readonly string[] color = new string[]
		{
			"white", "black", "yellow", "red", "blue", "brown", "green", "purple", "orange", "silver", "scarlet", "rainbow",
			"indigo", "ivory", "navy", "olive", "teal", "pink", "magenta", "maroon", "sienna", "gold", "golden"
		};

		public static readonly string[] adjective = new string[]
		{
			"abandoned", "aberrant", "accidentally", "aggressive", "aimless", "alien", "angry", "appropriate", "barbaric",
			"beacon", "big", "bitter", "bleeding", "brave", "brutal", "cheerful", "dancing", "dangerous", "dead", "deserted",
			"digital", "dirty", "disappointed", "discarded", "dreaded", "eastern", "eastern", "elastic", "empty", "endless",
			"essential", "eternal", "everyday", "fierce", "flaming", "flying", "forgotten", "forsaken", "freaky", "frozen",
			"full", "furious", "ghastly", "global", "gloomy", "grim", "gruesome", "gutsy", "helpless", "hidden", "hideous",
			"homeless", "hungry", "insane", "intense", "intensive", "itchy", "liquid", "lone", "lost", "meaningful", "modern",
			"monday's", "morbid", "moving", "needless", "nervous", "new", "next", "ninth", "nocturnal", "northernmost",
			"official", "old", "permanent", "persistent", "pointless", "pure", "quality", "random", "rare", "raw", "reborn",
			"remote", "restless", "rich", "risky", "rocky", "rough", "running", "rusty", "sad", "saturday's", "screaming",
			"serious", "severe", "silly", "skilled", "sleepy", "sliding", "small", "solid", "steamy", "stony", "stormy", "straw",
			"strawberry", "streaming", "strong", "subtle", "supersonic", "surreal", "tainted", "temporary", "third", "tidy",
			"timely", "unique", "vital", "western", "wild", "wooden", "worthy", "bitter", "boiling", "brave", "cloudy", "cold",
			"confidential", "dreadful", "dusty", "eager", "early", "grotesque ", "harsh", "heavy", "hollow", "hot", "husky",
			"icy", "late", "lonesome", "long", "lucky", "massive", "maximum", "minimum", "mysterious", "outstanding", "rapid",
			"rebel", "scattered", "shiny", "solid", "square", "steady", "steep", "sticky", "stormy", "strong", "sunday's",
			"swift", "tasty"
		};

		public static readonly string[] science = new string[]
		{
			"alarm", "albatross", "anaconda", "antique", "artificial", "autopsy", "autumn", "avenue", "backpack", "balcony",
			"barbershop", "boomerang", "bulldozer", "butter", "canal", "cloud", "clown", "coffin", "comic", "compass", "cosmic",
			"crayon", "creek", "crossbow", "dagger", "dinosaur", "dog", "donut", "door", "doorstop", "electrical", "electron",
			"eyelid", "firecracker", "fish", "flag", "flannel", "flea", "frostbite", "gravel", "haystack", "helium", "kangaroo",
			"lantern", "leather", "limousine", "lobster", "locomotive", "logbook", "longitude", "metaphor", "microphone",
			"monkey", "moose", "morning", "mountain", "mustard", "neutron", "nitrogen", "notorious", "obscure", "ostrich",
			"oyster", "parachute", "peasant", "pineapple", "plastic", "postal", "pottery", "proton", "puppet", "railroad",
			"rhinestone", "roadrunner", "rubber", "scarecrow", "scoreboard", "scorpion", "shower", "skunk", "sound", "street",
			"subdivision", "summer", "sunshine", "tea", "temple", "test", "tire", "tombstone", "toothbrush", "torpedo", "toupee",
			"trendy", "trombone", "tuba", "tuna", "tungsten", "vegetable", "venom", "vulture", "waffle", "warehouse", "waterbird",
			"weather", "weeknight", "windshield", "winter", "wrench", "xylophone", "alpha", "arm", "beam", "beta", "bird",
			"breeze", "burst", "cat", "cobra", "crystal", "drill", "eagle", "emerald", "epsilon", "finger", "fist", "foot", "fox",
			"galaxy", "gamma", "hammer", "heart", "hook", "hurricane", "iron", "jazz", "jupiter", "knife", "lama", "laser",
			"lion", "mars", "mercury", "moon", "moose", "neptune", "omega", "panther", "planet", "pluto", "plutonium", "poseidon",
			"python", "ray", "sapphire", "scissors", "screwdriver", "serpent", "sledgehammer", "smoke", "snake", "space",
			"spider", "star", "steel", "storm", "sun", "swallow", "tiger", "uranium", "venus", "viper", "wrench", "yard", "zeus"
		};

		public static readonly string[] elf_male = new string[]
		{
			"Abardon", "Acaman", "Achard", "Ackmard", "Agon", "Agnar", "Abdun", "Aidan", "Airis", "Aldaren", "Alderman", "Alkirk",
			"Amerdan", "Anfarc", "Aslan", "Actar", "Atgur", "Atlin", "Aldan", "Badek", "Baduk", "Bedic", "Beeron", "Bein",
			"Bithon", "Bohl", "Boldel", "Bolrock", "Bredin", "Bredock", "Breen", "tristan", "Bydern", "Cainon", "Calden", "Camon",
			"Cardon", "Casdon", "Celthric", "Cevelt", "Chamon", "Chidak", "Cibrock", "Cipyar", "Colthan", "Connell", "Cordale",
			"Cos", "Cyton", "Daburn", "Dawood", "Dak", "Dakamon", "Darkboon", "Dark", "Darg", "Darmor", "Darpick", "Dask",
			"Deathmar", "Derik", "Dismer", "Dokohan", "Doran", "Dorn", "Dosman", "Draghone", "Drit", "Driz", "Drophar", "Durmark",
			"Dusaro", "Eckard", "Efar", "Egmardern", "Elvar", "Elmut", "Eli", "Elik", "Elson", "Elthin", "Elbane", "Eldor",
			"Elidin", "Eloon", "Enro", "Erik", "Erim", "Eritai", "Escariet", "Espardo", "Etar", "Eldar", "Elthen", "Elfdorn",
			"Etran", "Eythil", "Fearlock", "Fenrirr", "Fildon", "Firdorn", "Florian", "Folmer", "Fronar", "Fydar", "Gai", "Galin",
			"Galiron", "Gametris", "Gauthus", "Gehardt", "Gemedes", "Gefirr", "Gibolt", "Geth", "Gom", "Gosform", "Gothar",
			"Gothor", "Greste", "Grim", "Gryni", "Gundir", "Gustov", "Halmar", "Haston", "Hectar", "Hecton", "Helmon", "Hermedes",
			"Hezaq", "Hildar", "Idon", "Ieli", "Ipdorn", "Ibfist", "Iroldak", "Ixen", "Ixil", "Izic", "Jamik", "Jethol", "Jihb",
			"Jibar", "Jhin", "Julthor", "Justahl", "Kafar", "Kaldar", "Kelar", "Keran", "Kib", "Kilden", "Kilbas", "Kildar",
			"Kimdar", "Kilder", "Koldof", "Kylrad", "Lackus", "Lacspor", "Lahorn", "Laracal", "Ledal", "Leith", "Lalfar", "Lerin",
			"Letor", "Lidorn", "Lich", "Loban", "Lox", "Ludok", "Ladok", "Lupin", "Lurd", "Mardin", "Markard", "Merklin",
			"Mathar", "Meldin", "Merdon", "Meridan", "Mezo", "Migorn", "Milen", "Mitar", "Modric", "Modum", "Madon", "Mafur",
			"Mujardin", "Mylo", "Mythik", "Nalfar", "Nadorn", "Naphazw", "Neowald", "Nildale", "Nizel", "Nilex", "Niktohal",
			"Niro", "Nothar", "Nathon", "Nadale", "Nythil", "Ozhar", "Oceloth", "Odeir", "Ohmar", "Orin", "Oxpar", "Othelen",
			"Padan", "Palid", "Palpur", "Peitar", "Pendus", "Penduhl", "Pildoor", "Puthor", "Phar", "Phalloz", "Qidan", "Quid",
			"Qupar", "Randar", "Raydan", "Reaper", "Relboron", "Riandur", "Rikar", "Rismak", "Riss", "Ritic", "Ryodan", "Rysdan",
			"Rythen", "Rythorn", "Sabalz", "Sadaron", "Safize", "Samon", "Samot", "Secor", "Sedar", "Senic", "Santhil", "Sermak",
			"Seryth", "Seth", "Shane", "Shard", "Shardo", "Shillen", "Silco", "Sildo", "Silpal", "Sithik", "Soderman", "Sothale",
			"Staph", "Suktar", "zuth", "Sutlin", "Syr", "Syth", "Sythril", "Talberon", "Telpur", "Temil", "Tamilfist", "Tempist",
			"Teslanar", "Tespan", "Tesio", "Thiltran", "Tholan", "Tibers", "Tibolt", "Thol", "Tildor", "Tilthan", "Tobaz",
			"Todal", "Tothale", "Touck", "Tok", "Tuscan", "Tusdar", "Tyden", "Uerthe", "Uhmar", "Uhrd", "Updar", "Uther", "Vacon",
			"Valker", "Valyn", "Vectomon", "Veldar", "Velpar", "Vethelot", "Vildher", "Vigoth", "Vilan", "Vildar", "Vi", "Vinkol",
			"Virdo", "Voltain", "Wanar", "Wekmar", "Weshin", "Witfar", "Wrathran", "Waytel", "Wathmon", "Wider", "Wyeth",
			"Xandar", "Xavor", "Xenil", "Xelx", "Xithyl", "Yerpal", "Yesirn", "Ylzik", "Zak", "Zek", "Zerin", "Zestor", "Zidar",
			"Zigmal", "Zilex", "Zilz", "Zio", "Zotar", "Zutar", "Zytan"
		};

		public static readonly string[] elf_female = new string[]
		{
			"Acele Acholate", "Ada", "Adiannon", "Adorra", "Ahanna", "Akara", "Akassa", "Akia", "Amara", "Amarisa", "Amarizi",
			"Ana", "Andonna", "Ariannona", "Arina", "Arryn", "Asada", "Awnia", "Ayne", "Basete", "Bathelie", "Bethel", "Brana",
			"Brynhilde", "Calene", "Calina", "Celestine", "Corda", "Enaldie", "Enoka", "Enoona", "Errinaya", "Fayne", "Frederika",
			"Frida", "Gvene", "Gwethana", "Helenia", "Hildandi", "Helvetica", "Idona", "Irina", "Irene", "Illia", "Irona",
			"Justalyne", "Kassina", "Kilia", "Kressara", "Laela", "Laenaya", "Lelani", "Luna", "Linyah", "Lyna", "Lynessa",
			"Mehande", "Melisande", "Midiga", "Mirayam", "Mylene", "Naria", "Narisa", "Nelena", "Nimaya", "Nymia", "Ochala",
			"Olivia", "Onathe", "Parthinia", "Philadona", "Prisane", "Rhyna", "Rivatha", "Ryiah", "Sanata", "Sathe", "Senira",
			"Sennetta", "Serane", "Sevestra", "Sidara", "Sidathe", "Sina", "Sunete", "Synestra", "Sythini", "zena", "Tabithi",
			"Tomara", "Teressa", "Tonica", "Thea", "Teressa", "Urda", "Usara", "Useli", "Unessa", "ursula", "Venessa", "Wanera",
			"Wellisa", "yeta", "Ysane", "Yve", "Yviene", "Zana", "Zathe", "Zecele", "Zenobe", "Zema", "Zestia", "Zilka", "Zoucka",
			"Zona", "Zyneste", "Zynoa"
		};

		public static readonly string[] elf_surname = new string[]
		{
			"Abardon", "Acaman", "Achard", "Ackmard", "Agon", "Agnar", "Aldan", "Abdun", "Aidan", "Airis", "Aldaren", "Alderman",
			"Alkirk", "Amerdan", "Anfarc", "Aslan", "Actar", "Atgur", "Atlin", "Badek", "Baduk", "Bedic", "Beeron", "Bein",
			"Bithon", "Bohl", "Boldel", "Bolrock", "Bredin", "Bredock", "Breen", "tristan", "Bydern", "Cainon", "Calden", "Camon",
			"Cardon", "Casdon", "Celthric", "Cevelt", "Chamon", "Chidak", "Cibrock", "Cipyar", "Colthan", "Connell", "Cordale",
			"Cos", "Cyton", "Daburn", "Dawood", "Dak", "Dakamon", "Darkboon", "Dark", "Dark", "Darmor", "Darpick", "Dask",
			"Deathmar", "Derik", "Dismer", "Dokohan", "Doran", "Dorn", "Dosman", "Draghone", "Drit", "Driz", "Drophar", "Durmark",
			"Dusaro", "Eckard", "Efar", "Egmardern", "Elvar", "Elmut", "Eli", "Elik", "Elson", "Elthin", "Elbane", "Eldor",
			"Elidin", "Eloon", "Enro", "Erik", "Erim", "Eritai", "Escariet", "Espardo", "Etar", "Eldar", "Elthen", "Etran",
			"Eythil", "Fearlock", "Fenrirr", "Fildon", "Firdorn", "Florian", "Folmer", "Fronar", "Fydar", "Gai", "Galin",
			"Galiron", "Gametris", "Gauthus", "Gehardt", "Gemedes", "Gefirr", "Gibolt", "Geth", "Gom", "Gosform", "Gothar",
			"Gothor", "Greste", "Grim", "Gryni", "Gundir", "Gustov", "Halmar", "Haston", "Hectar", "Hecton", "Helmon", "Hermedes",
			"Hezaq", "Hildar", "Idon", "Ieli", "Ipdorn", "Ibfist", "Iroldak", "Ixen", "Ixil", "Izic", "Jamik", "Jethol", "Jihb",
			"Jibar", "Jhin", "Julthor", "Justahl", "Kafar", "Kaldar", "Kelar", "Keran", "Kib", "Kilden", "Kilbas", "Kildar",
			"Kimdar", "Kilder", "Koldof", "Kylrad", "Lackus", "Lacspor", "Lahorn", "Laracal", "Ledal", "Leith", "Lalfar", "Lerin",
			"Letor", "Lidorn", "Lich", "Loban", "Lox", "Ludok", "Ladok", "Lupin", "Lurd", "Mardin", "Markard", "Merklin",
			"Mathar", "Meldin", "Merdon", "Meridan", "Mezo", "Migorn", "Milen", "Mitar", "Modric", "Modum", "Madon", "Mafur",
			"Mujardin", "Mylo", "Mythik", "Nalfar", "Nadorn", "Naphazw", "Neowald", "Nildale", "Nizel", "Nilex", "Niktohal",
			"Niro", "Nothar", "Nathon", "Nadale", "Nythil", "Ozhar", "Oceloth", "Odeir", "Ohmar", "Orin", "Oxpar", "Othelen",
			"Padan", "Palid", "Palpur", "Peitar", "Pendus", "Penduhl", "Pildoor", "Puthor", "Phar", "Phalloz", "Qidan", "Quid",
			"Qupar", "Randar", "Raydan", "Reaper", "Relboron", "Riandur", "Rikar", "Rismak", "Riss", "Ritic", "Ryodan", "Rysdan",
			"Rythen", "Rythorn", "Sabalz", "Sadaron", "Safize", "Samon", "Samot", "Secor", "Sedar", "Senic", "Santhil", "Sermak",
			"Seryth", "Seth", "Shane", "Shard", "Shardo", "Shillen", "Silco", "Sildo", "Silpal", "Sithik", "Soderman", "Sothale",
			"Staph", "Suktar", "zuth", "Sutlin", "Syr", "Syth", "Sythril", "Talberon", "Telpur", "Temil", "Tamilfist", "Tempist",
			"Teslanar", "Tespan", "Tesio", "Thiltran", "Tholan", "Tibers", "Tibolt", "Thol", "Tildor", "Tilthan", "Tobaz",
			"Todal", "Tothale", "Touck", "Tok", "Tuscan", "Tusdar", "Tyden", "Uerthe", "Uhmar", "Uhrd", "Updar", "Uther", "Vacon",
			"Valker", "Valyn", "Vectomon", "Veldar", "Velpar", "Vethelot", "Vildher", "Vigoth", "Vilan", "Vildar", "Vi", "Vinkol",
			"Virdo", "Voltain", "Wanar", "Wekmar", "Weshin", "Witfar", "Wrathran", "Waytel", "Wathmon", "Wider", "Wyeth",
			"Xandar", "Xavor", "Xenil", "Xelx", "Xithyl", "Yerpal", "Yesirn", "Ylzik", "Zak", "Zek", "Zerin", "Zestor", "Zidar",
			"Zigmal", "Zilex", "Zilz", "Zio", "Zotar", "Zutar", "Zytan"
		};

		public static readonly string[] fantasy_male = new string[]
		{
			"Abardon", "Acaman", "Achard", "Ackmard", "Agon", "Agnar", "Abdun", "Aidan", "Airis", "Aldaren", "Alderman", "Alkirk",
			"Amerdan", "Anfarc", "Aslan", "Actar", "Atgur", "Atlin", "Aldan", "Badek", "Baduk", "Bedic", "Beeron", "Bein",
			"Bithon", "Bohl", "Boldel", "Bolrock", "Bredin", "Bredock", "Breen", "tristan", "Bydern", "Cainon", "Calden", "Camon",
			"Cardon", "Casdon", "Celthric", "Cevelt", "Chamon", "Chidak", "Cibrock", "Cipyar", "Colthan", "Connell", "Cordale",
			"Cos", "Cyton", "Daburn", "Dawood", "Dak", "Dakamon", "Darkboon", "Dark", "Darg", "Darmor", "Darpick", "Dask",
			"Deathmar", "Derik", "Dismer", "Dokohan", "Doran", "Dorn", "Dosman", "Draghone", "Drit", "Driz", "Drophar", "Durmark",
			"Dusaro", "Eckard", "Efar", "Egmardern", "Elvar", "Elmut", "Eli", "Elik", "Elson", "Elthin", "Elbane", "Eldor",
			"Elidin", "Eloon", "Enro", "Erik", "Erim", "Eritai", "Escariet", "Espardo", "Etar", "Eldar", "Elthen", "Etran",
			"Eythil", "Fearlock", "Fenrirr", "Fildon", "Firdorn", "Florian", "Folmer", "Fronar", "Fydar", "Gai", "Galin",
			"Galiron", "Gametris", "Gauthus", "Gehardt", "Gemedes", "Gefirr", "Gibolt", "Geth", "Gom", "Gosform", "Gothar",
			"Gothor", "Greste", "Grim", "Gryni", "Gundir", "Gustov", "Halmar", "Haston", "Hectar", "Hecton", "Helmon", "Hermedes",
			"Hezaq", "Hildar", "Idon", "Ieli", "Ipdorn", "Ibfist", "Iroldak", "Ixen", "Ixil", "Izic", "Jamik", "Jethol", "Jihb",
			"Jibar", "Jhin", "Julthor", "Justahl", "Kafar", "Kaldar", "Kelar", "Keran", "Kib", "Kilden", "Kilbas", "Kildar",
			"Kimdar", "Kilder", "Koldof", "Kylrad", "Lackus", "Lacspor", "Lahorn", "Laracal", "Ledal", "Leith", "Lalfar", "Lerin",
			"Letor", "Lidorn", "Lich", "Loban", "Lox", "Ludok", "Ladok", "Lupin", "Lurd", "Mardin", "Markard", "Merklin",
			"Mathar", "Meldin", "Merdon", "Meridan", "Mezo", "Migorn", "Milen", "Mitar", "Modric", "Modum", "Madon", "Mafur",
			"Mujardin", "Mylo", "Mythik", "Nalfar", "Nadorn", "Naphazw", "Neowald", "Nildale", "Nizel", "Nilex", "Niktohal",
			"Niro", "Nothar", "Nathon", "Nadale", "Nythil", "Ozhar", "Oceloth", "Odeir", "Ohmar", "Orin", "Oxpar", "Othelen",
			"Padan", "Palid", "Palpur", "Peitar", "Pendus", "Penduhl", "Pildoor", "Puthor", "Phar", "Phalloz", "Qidan", "Quid",
			"Qupar", "Randar", "Raydan", "Reaper", "Relboron", "Riandur", "Rikar", "Rismak", "Riss", "Ritic", "Ryodan", "Rysdan",
			"Rythen", "Rythorn", "Sabalz", "Sadaron", "Safize", "Samon", "Samot", "Secor", "Sedar", "Senic", "Santhil", "Sermak",
			"Seryth", "Seth", "Shane", "Shard", "Shardo", "Shillen", "Silco", "Sildo", "Silpal", "Sithik", "Soderman", "Sothale",
			"Staph", "Suktar", "zuth", "Sutlin", "Syr", "Syth", "Sythril", "Talberon", "Telpur", "Temil", "Tamilfist", "Tempist",
			"Teslanar", "Tespan", "Tesio", "Thiltran", "Tholan", "Tibers", "Tibolt", "Thol", "Tildor", "Tilthan", "Tobaz",
			"Todal", "Tothale", "Touck", "Tok", "Tuscan", "Tusdar", "Tyden", "Uerthe", "Uhmar", "Uhrd", "Updar", "Uther", "Vacon",
			"Valker", "Valyn", "Vectomon", "Veldar", "Velpar", "Vethelot", "Vildher", "Vigoth", "Vilan", "Vildar", "Vi", "Vinkol",
			"Virdo", "Voltain", "Wanar", "Wekmar", "Weshin", "Witfar", "Wrathran", "Waytel", "Wathmon", "Wider", "Wyeth",
			"Xandar", "Xavor", "Xenil", "Xelx", "Xithyl", "Yerpal", "Yesirn", "Ylzik", "Zak", "Zek", "Zerin", "Zestor", "Zidar",
			"Zigmal", "Zilex", "Zilz", "Zio", "Zotar", "Zutar", "Zytan"
		};

		public static readonly string[] fantasy_female = new string[]
		{
			"Acele Acholate", "Ada", "Adiannon", "Adorra", "Ahanna", "Akara", "Akassa", "Akia", "Amara", "Amarisa", "Amarizi",
			"Ana", "Andonna", "Ariannona", "Arina", "Arryn", "Asada", "Awnia", "Ayne", "Basete", "Bathelie", "Bethel", "Brana",
			"Brynhilde", "Calene", "Calina", "Celestine", "Corda", "Enaldie", "Enoka", "Enoona", "Errinaya", "Fayne", "Frederika",
			"Frida", "Gvene", "Gwethana", "Helenia", "Hildandi", "Helvetica", "Idona", "Irina", "Irene", "Illia", "Irona",
			"Justalyne", "Kassina", "Kilia", "Kressara", "Laela", "Laenaya", "Lelani", "Luna", "Linyah", "Lyna", "Lynessa",
			"Mehande", "Melisande", "Midiga", "Mirayam", "Mylene", "Naria", "Narisa", "Nelena", "Nimaya", "Nymia", "Ochala",
			"Olivia", "Onathe", "Parthinia", "Philadona", "Prisane", "Rhyna", "Rivatha", "Ryiah", "Sanata", "Sathe", "Senira",
			"Sennetta", "Serane", "Sevestra", "Sidara", "Sidathe", "Sina", "Sunete", "Synestra", "Sythini", "zena", "Tabithi",
			"Tomara", "Teressa", "Tonica", "Thea", "Teressa", "Urda", "Usara", "Useli", "Unessa", "ursula", "Venessa", "Wanera",
			"Wellisa", "yeta", "Ysane", "Yve", "Yviene", "Zana", "Zathe", "Zecele", "Zenobe", "Zema", "Zestia", "Zilka", "Zoucka",
			"Zona", "Zyneste", "Zynoa"
		};

		public static readonly string[] fantasy_surname = new string[]
		{
			"Abardon", "Acaman", "Achard", "Ackmard", "Agon", "Agnar", "Aldan", "Abdun", "Aidan", "Airis", "Aldaren", "Alderman",
			"Alkirk", "Amerdan", "Anfarc", "Aslan", "Actar", "Atgur", "Atlin", "Badek", "Baduk", "Bedic", "Beeron", "Bein",
			"Bithon", "Bohl", "Boldel", "Bolrock", "Bredin", "Bredock", "Breen", "tristan", "Bydern", "Cainon", "Calden", "Camon",
			"Cardon", "Casdon", "Celthric", "Cevelt", "Chamon", "Chidak", "Cibrock", "Cipyar", "Colthan", "Connell", "Cordale",
			"Cos", "Cyton", "Daburn", "Dawood", "Dak", "Dakamon", "Darkboon", "Dark", "Darmor", "Darpick", "Dask", "Deathmar",
			"Derik", "Dismer", "Dokohan", "Doran", "Dorn", "Dosman", "Draghone", "Drit", "Driz", "Drophar", "Durmark", "Dusaro",
			"Eckard", "Efar", "Egmardern", "Elvar", "Elmut", "Eli", "Elik", "Elson", "Elthin", "Elbane", "Eldor", "Elidin",
			"Eloon", "Enro", "Erik", "Erim", "Eritai", "Escariet", "Espardo", "Etar", "Eldar", "Elthen", "Etran", "Eythil",
			"Fearlock", "Fenrirr", "Fildon", "Firdorn", "Florian", "Folmer", "Fronar", "Fydar", "Gai", "Galin", "Galiron",
			"Gametris", "Gauthus", "Gehardt", "Gemedes", "Gefirr", "Gibolt", "Geth", "Gom", "Gosform", "Gothar", "Gothor",
			"Greste", "Grim", "Gryni", "Gundir", "Gustov", "Halmar", "Haston", "Hectar", "Hecton", "Helmon", "Hermedes", "Hezaq",
			"Hildar", "Idon", "Ieli", "Ipdorn", "Ibfist", "Iroldak", "Ixen", "Ixil", "Izic", "Jamik", "Jethol", "Jihb", "Jibar",
			"Jhin", "Julthor", "Justahl", "Kafar", "Kaldar", "Kelar", "Keran", "Kib", "Kilden", "Kilbas", "Kildar", "Kimdar",
			"Kilder", "Koldof", "Kylrad", "Lackus", "Lacspor", "Lahorn", "Laracal", "Ledal", "Leith", "Lalfar", "Lerin", "Letor",
			"Lidorn", "Lich", "Loban", "Lox", "Ludok", "Ladok", "Lupin", "Lurd", "Mardin", "Markard", "Merklin", "Mathar",
			"Meldin", "Merdon", "Meridan", "Mezo", "Migorn", "Milen", "Mitar", "Modric", "Modum", "Madon", "Mafur", "Mujardin",
			"Mylo", "Mythik", "Nalfar", "Nadorn", "Naphazw", "Neowald", "Nildale", "Nizel", "Nilex", "Niktohal", "Niro", "Nothar",
			"Nathon", "Nadale", "Nythil", "Ozhar", "Oceloth", "Odeir", "Ohmar", "Orin", "Oxpar", "Othelen", "Padan", "Palid",
			"Palpur", "Peitar", "Pendus", "Penduhl", "Pildoor", "Puthor", "Phar", "Phalloz", "Qidan", "Quid", "Qupar", "Randar",
			"Raydan", "Reaper", "Relboron", "Riandur", "Rikar", "Rismak", "Riss", "Ritic", "Ryodan", "Rysdan", "Rythen",
			"Rythorn", "Sabalz", "Sadaron", "Safize", "Samon", "Samot", "Secor", "Sedar", "Senic", "Santhil", "Sermak", "Seryth",
			"Seth", "Shane", "Shard", "Shardo", "Shillen", "Silco", "Sildo", "Silpal", "Sithik", "Soderman", "Sothale", "Staph",
			"Suktar", "zuth", "Sutlin", "Syr", "Syth", "Sythril", "Talberon", "Telpur", "Temil", "Tamilfist", "Tempist",
			"Teslanar", "Tespan", "Tesio", "Thiltran", "Tholan", "Tibers", "Tibolt", "Thol", "Tildor", "Tilthan", "Tobaz",
			"Todal", "Tothale", "Touck", "Tok", "Tuscan", "Tusdar", "Tyden", "Uerthe", "Uhmar", "Uhrd", "Updar", "Uther", "Vacon",
			"Valker", "Valyn", "Vectomon", "Veldar", "Velpar", "Vethelot", "Vildher", "Vigoth", "Vilan", "Vildar", "Vi", "Vinkol",
			"Virdo", "Voltain", "Wanar", "Wekmar", "Weshin", "Witfar", "Wrathran", "Waytel", "Wathmon", "Wider", "Wyeth",
			"Xandar", "Xavor", "Xenil", "Xelx", "Xithyl", "Yerpal", "Yesirn", "Ylzik", "Zak", "Zek", "Zerin", "Zestor", "Zidar",
			"Zigmal", "Zilex", "Zilz", "Zio", "Zotar", "Zutar", "Zytan"
		};

		#endregion

		#region Имена набор 2

		public static readonly string[] rus_surname = new string[]
		{
			"Иванов", "Смирнов", "Кузнецов", "Попов", "Васильев", "Петров", "Соколов", "Михайлов", "Новиков", "Федоров",
			"Морозов", "Волков", "Алексеев", "Лебедев", "Семенов", "Егоров", "Павлов", "Козлов", "Степанов", "Николаев", "Орлов",
			"Андреев", "Макаров", "Никитин", "Захаров", "Зайцев", "Соловьев", "Борисов", "Яковлев", "Григорьев", "Романов",
			"Воробьев", "Сергеев", "Кузьмин", "Фролов", "Александров", "Дмитриев", "Королев", "Гусев", "Киселев", "Ильин",
			"Максимов", "Поляков", "Сорокин", "Виноградов", "Ковалев", "Белов", "Медведев", "Антонов", "Тарасов", "Жуков",
			"Баранов", "Филиппов", "Комаров", "Давыдов", "Беляев", "Герасимов", "Богданов", "Осипов", "Сидоров", "Матвеев",
			"Титов", "Марков", "Миронов", "Крылов", "Куликов", "Карпов", "Власов", "Мельников", "Денисов", "Гаврилов", "Тихонов",
			"Казаков", "Афанасьев", "Данилов", "Савельев", "Тимофеев", "Фомин", "Чернов", "Абрамов", "Мартынов", "Ефимов",
			"Федотов", "Щербаков", "Назаров", "Калинин", "Исаев", "Чернышев", "Быков", "Маслов", "Родионов", "Коновалов",
			"Лазарев", "Воронин", "Климов", "Филатов", "Пономарев", "Голубев", "Кудрявцев", "Прохоров", "Наумов", "Потапов",
			"Журавлев", "Овчинников", "Трофимов", "Леонов", "Соболев", "Ермаков", "Колесников", "Гончаров", "Емельянов",
			"Никифоров", "Грачев", "Котов", "Гришин", "Ефремов", "Архипов", "Громов", "Кириллов", "Малышев", "Панов", "Моисеев",
			"Румянцев", "Акимов", "Кондратьев", "Бирюков", "Горбунов", "Анисимов", "Еремин", "Тихомиров", "Галкин", "Лукьянов",
			"Михеев", "Скворцов", "Юдин", "Белоусов", "Нестеров", "Симонов", "Прокофьев", "Харитонов", "Князев", "Цветков",
			"Левин", "Митрофанов", "Воронов", "Аксенов", "Софронов", "Мальцев", "Логинов", "Горшков", "Савин", "Краснов",
			"Майоров", "Демидов", "Елисеев", "Рыбаков", "Сафонов", "Плотников", "Демин", "Хохлов", "Фадеев", "Молчанов",
			"Игнатов", "Литвинов", "Ершов", "Ушаков", "Дементьев", "Рябов", "Мухин", "Калашников", "Леонтьев", "Лобанов", "Кузин",
			"Корнеев", "Евдокимов", "Бородин", "Платонов", "Некрасов", "Балашов", "Бобров", "Жданов", "Блинов", "Игнатьев",
			"Коротков", "Муравьев", "Крюков", "Беляков", "Богомолов", "Дроздов", "Лавров", "Зуев", "Петухов", "Ларин", "Никулин",
			"Серов", "Терентьев", "Зотов", "Устинов", "Фокин", "Самойлов", "Константинов", "Сахаров", "Шишкин", "Самсонов",
			"Черкасов", "Чистяков", "Носов", "Спиридонов", "Карасев", "Авдеев", "Воронцов", "Зверев", "Владимиров", "Селезнев",
			"Нечаев", "Кудряшов", "Седов", "Фирсов", "Андрианов", "Панин", "Головин", "Терехов", "Ульянов", "Шестаков", "Агеев",
			"Никонов", "Селиванов", "Баженов", "Гордеев", "Кожевников", "Пахомов", "Зимин", "Костин", "Широков", "Филимонов",
			"Ларионов", "Овсянников", "Сазонов", "Суворов", "Нефедов", "Корнилов", "Любимов", "Львов", "Горбачев", "Копылов",
			"Лукин", "Токарев", "Кулешов", "Шилов", "Большаков", "Панкратов", "Родин", "Шаповалов", "Покровский", "Бочаров",
			"Никольский", "Маркин", "Горелов", "Агафонов", "Березин", "Ермолаев", "Зубков", "Куприянов", "Трифонов",
			"Масленников", "Круглов", "Третьяков", "Колосов", "Рожков", "Артамонов", "Шмелев", "Лаптев", "Лапшин", "Федосеев",
			"Зиновьев", "Зорин", "Уткин", "Столяров", "Зубов", "Ткачев", "Дорофеев", "Антипов", "Завьялов", "Свиридов",
			"Золотарев", "Кулаков", "Мещеряков", "Макеев", "Дьяконов", "Гуляев", "Петровский", "Бондарев", "Поздняков",
			"Панфилов", "Кочетков", "Суханов", "Рыжов", "Старостин", "Калмыков", "Колесов", "Золотов", "Кравцов", "Субботин",
			"Шубин", "Щукин", "Лосев", "Винокуров", "Лапин", "Парфенов", "Исаков", "Голованов", "Коровин", "Розанов", "Артемов",
			"Козырев", "Русаков", "Алешин", "Крючков", "Булгаков", "Кошелев", "Сычев", "Синицын", "Черных", "Рогов", "Кононов",
			"Лаврентьев", "Евсеев", "Пименов", "Пантелеев", "Горячев", "Аникин", "Лопатин", "Рудаков", "Одинцов", "Серебряков",
			"Панков", "Дегтярев", "Орехов", "Царев", "Шувалов", "Кондрашов", "Горюнов", "Дубровин", "Голиков", "Курочкин",
			"Латышев", "Севастьянов", "Вавилов", "Ерофеев", "Сальников", "Клюев", "Носков", "Озеров", "Кольцов", "Комиссаров",
			"Меркулов", "Киреев", "Хомяков", "Булатов", "Ананьев", "Буров", "Шапошников", "Дружинин", "Островский", "Шевелев",
			"Долгов", "Суслов", "Шевцов", "Пастухов", "Рубцов", "Бычков", "Глебов", "Ильинский", "Успенский", "Дьяков", "Кочетов",
			"Вишневский", "Высоцкий", "Глухов", "Дубов", "Бессонов", "Ситников", "Астафьев", "Мешков", "Шаров", "Яшин",
			"Козловский", "Туманов", "Басов", "Корчагин", "Болдырев", "Олейников", "Чумаков", "Фомичев", "Губанов", "Дубинин",
			"Шульгин", "Касаткин", "Пирогов", "Семин", "Трошин", "Горохов", "Стариков", "Щеглов", "Фетисов", "Колпаков",
			"Чесноков", "Зыков", "Верещагин", "Минаев", "Руднев", "Троицкий", "Окулов", "Ширяев", "Малинин", "Черепанов",
			"Измайлов", "Алехин", "Зеленин", "Касьянов", "Пугачев", "Павловский", "Чижов", "Кондратов", "Воронков", "Капустин",
			"Сотников", "Демьянов", "Косарев", "Беликов", "Сухарев", "Белкин", "Беспалов", "Кулагин", "Савицкий", "Жаров",
			"Хромов", "Еремеев", "Карташов", "Астахов", "Русанов", "Сухов", "Вешняков", "Волошин", "Козин", "Худяков", "Жилин",
			"Малахов", "Сизов", "Ежов", "Толкачев", "Анохин", "Вдовин", "Бабушкин", "Усов", "Лыков", "Горлов", "Коршунов",
			"Маркелов", "Постников", "Черный", "Дорохов", "Свешников", "Гущин", "Калугин", "Блохин", "Сурков", "Кочергин",
			"Греков", "Казанцев", "Швецов", "Ермилов", "Парамонов", "Агапов", "Минин", "Корнев", "Черняев", "Гуров", "Ермолов",
			"Сомов", "Добрынин", "Барсуков", "Глушков", "Чеботарев", "Москвин", "Уваров", "Безруков", "Муратов", "Раков",
			"Снегирев", "Гладков", "Злобин", "Моргунов", "Поликарпов", "Рябинин", "Судаков", "Кукушкин", "Калачев", "Грибов",
			"Елизаров", "Звягинцев", "Корольков", "Федосов"
		};

		public static readonly string[] rus_male = new string[]
		{
			"Авангард", "Август", "Августин", "Авенир", "Авксентий", "Аврор", "Адам", "Адольф", "Адонис", "Алевтин", "Александр",
			"Алексей", "Альберт", "Альбин", "Альфред", "Анастасий", "Анатолий", "Андрей", "Анис", "Антон", "Антонин", "Антуан",
			"Аполлинарий", "Аполлон", "Аргент", "Аристарх", "Аркадий", "Арнольд", "Арсен", "Арсений", "Артём", "Артур", "Атеист",
			"Афанасий", "Бажен", "Бенедикт", "Богдан", "Боеслав", "Болеслав", "Боримир", "Борис", "Борислав", "Бронислав",
			"Будимир", "Булат", "Вадим", "Валентин", "Валерий", "Вальтер", "Василий", "Василько", "Велимир", "Велислав", "Велор",
			"Венедикт", "Вениамин", "Виктор", "Вилен", "Виссарион", "Виталий", "Витольд", "Влад", "Владилен", "Владимир",
			"Владислав", "Владлен", "Воин", "Воислав", "Володар", "Вольдемар", "Вольмир", "Всеволод", "Всемил", "Вячеслав",
			"Гавриил", "Галактион", "Гарри", "Гелиан", "Гений", "Геннадий", "Георгий", "Герман", "Гертруд", "Глеб", "Гордей",
			"Горимир", "Горислав", "Гранит", "Григорий", "Давыд", "Дамир", "Дан", "Даниил", "Дар", "Декабрий", "Денис",
			"Джеральд", "Джозеф", "Джон", "Дионисий", "Дмитрий", "Добрыня", "Дональт", "Донат", "Евгений", "Евдоким", "Егор",
			"Еруслан", "Ефим", "Ждан", "Захар", "Зиновий", "Зорий", "Ибрагим", "Иван", "Игнатий", "Игорь", "Исидор", "Июлий",
			"Казимир", "Карл", "Касьян", "Ким", "Киприан", "Кир", "Кирилл", "Клавдий", "Клемент", "Климент", "Климентий",
			"Колумбий", "Кузьма", "Куприян", "Лавр", "Лаврентий", "Лазарь", "Ларион", "Лев", "Леонард", "Леонид", "Леонтий",
			"Лука", "Лукьян", "Любим", "Любомир", "Люксен", "Маврикий", "Май", "Майслав", "Макар", "Макс", "Максим",
			"Максимильян", "Милий", "Милонег", "Милослав", "Мир", "Мирон", "Мирослав", "Михаил", "Модест", "Моисей", "Монолит",
			"Назар", "Натан", "Наум", "Неон", "Неонил", "Нестор", "Никандр", "Норд", "Овидий", "Одиссей", "Октавиан", "Октябрин",
			"Октябрь", "Олег", "Орест", "Осип", "Оскар", "Павел", "Палладий", "Пантелеймон", "Панфил", "Пересвет", "Пётр",
			"Прохор", "Радий", "Радим", "Радислав", "Радомир", "Савва", "Савелий", "Свет", "Светлан", "Светозар", "Светослав",
			"Святогор", "Святополк", "Святослав", "Север", "Северин", "Северьян", "Северян", "Семён", "Серафим", "Сергей",
			"Сигизмунд", "Сталь", "Станислав", "Степан", "Тарас", "Теймураз", "Тристан", "Трифон", "Трофим", "Фадей", "Февралин",
			"Фёдор", "Федор", "Феликс", "Филимон", "Филипп", "Флегонт", "Флорентий", "Флоренц", "Флорин", "Фрол", "Харитон",
			"Храбр", "Христоф", "Эдуард", "Электрон", "Эльбрус", "Энергий", "Эрнест", "Ювеналий", "Юджин", "Юлиан", "Юлий",
			"Юпитер", "Юрий", "Яков", "Ян", "Януарий", "Яромир", "Ярополк", "Ярослав"
		};

		public static readonly string[] rus_female = new string[]
		{
			"Августа", "Авдотья", "Аврелия", "Аврора", "Агапия", "Агата", "Аглаида", "Аглая", "Агнеса", "Агния", "Агриппина",
			"Ада", "Адель", "Аза", "Азалия", "Аида", "Акилина", "Аксинья", "Алевтина", "Александра", "Алёна", "Алина", "Алиса",
			"Алла", "Альбина", "Анастасия", "Анатолия", "Ангелина", "Анжела", "Анимаиса", "Анисия", "Анита", "Анна", "Антонина",
			"Антония", "Анфиса", "Аполлинария", "Ариадна", "Арина", "Аркадия", "Арсения", "Артемия", "Астра", "Астрид",
			"Афанасия", "Афродита", "Аэлита", "Аэлла", "Бажена", "Беата", "Беатриса", "Бела", "Белла", "Берта", "Богдана",
			"Болеслава", "Борислава", "Бронислава", "Валентина", "Валерия", "Ванда", "Варвара", "Василина", "Василиса", "Васса",
			"Вацлава", "Венера", "Вера", "Вероника", "Веселина", "Веста", "Видана", "Викторина", "Виктория", "Вилена", "Виола",
			"Виринея", "Виталия", "Влада", "Владилена", "Владимира", "Владислава", "Владлена", "Власта", "Воля", "Всеслава",
			"Гайя", "Гали", "Галина", "Ганна", "Гаяна", "Гелена", "Гелия", "Гелла", "Гертруда", "Глафира", "Гликерия", "Глория",
			"Голуба", "Горислава", "Дайна", "Дана", "Дарья", "Дарина", "Дарьяна", "Декабрина", "Дея", "Джульетта", "Диана",
			"Дина", "Диодора", "Дионисия", "Добрава", "Домна", "Домника", "Дорофея", "Ева", "Евгения", "Евдокия", "Евпраксия",
			"Екатерина", "Елена", "Елизавета", "Ефимия", "Ефросиния", "Жанна", "Ждана", "Зарина", "Звенислава", "Зинаида",
			"Зиновия", "Злата", "Зоя", "Иванна", "Ида", "Илария", "Инга", "Инесса", "Инна", "Иоанна", "Иона", "Ипатия",
			"Ипполита", "Ираида", "Ироида", "Ирина", "Исидора", "Искра", "Ифигения", "Ия", "Капитолина", "Каролина", "Катерина",
			"Кира", "Кирилла", "Клавдия", "Клара", "Клариса", "Клеопатра", "Конкордия", "Констанция", "Кристина", "Ксения",
			"Лада", "Лариса", "Лениана", "Ленина", "Леонида", "Леонила", "Леонтия", "Леся", "Ливия", "Лидия", "Лилиана", "Лилия",
			"Лина", "Любава", "Любовь", "Любомира", "Людмила", "Мавра", "Магда", "Магдалина", "Мадлен", "Майя", "Мальвина",
			"Маргарита", "Марина", "Мария", "Мари", "Марта", "Марфа", "Матильда", "Матрёна", "Мелания", "Милада", "Милана",
			"Милица", "Милослава", "Мира", "Мирра", "Мирослава", "Митродора", "Млада", "Мстислава", "Муза", "Надежда", "Надия",
			"Нана", "Настасья", "Наталья", "Нелли", "Неонила", "Ника", "Нина", "Нинель", "Новелла", "Нора", "Оксана", "Октавия",
			"Октябрина", "Олеся", "Олимпиада", "Олимпия", "Ольга", "Павла", "Павлина", "Платонида", "Поликсена", "Полина",
			"Правдина", "Прасковья", "Рада", "Радмила", "Раиса", "Ревмира", "Регина", "Рената", "Римма", "Рогнеда", "Роза",
			"Розалия", "Розана", "Ростислава", "Руслана", "Руфина", "Сабина", "Саломея", "Светлана", "Светозара", "Светослава",
			"Свобода", "Святослава", "Севастьяна", "Северина", "Селена", "Серафима", "Слава", "Славяна", "Снежана", "Софья",
			"Станислава", "Стелла", "Степанида", "Стефания", "Сусанна", "Сюзанна", "Таира", "Таисия", "Тамара", "Тамила",
			"Татьяна", "Ульяна", "Услада", "Устинья", "Фаина", "Феликсана", "Фелицата", "Фелиция", "Федора", "Феодосия",
			"Филадельфия", "Флавия", "Флора", "Флорентина", "Флоренция", "Флориана", "Фотина", "Харита", "Харитина", "Хиония",
			"Христина", "Чеслава", "Эвридика", "Элеонора", "Эльвира", "Эльмира", "Эльза", "Эмма", "Эрика", "Юлиана", "Юлия",
			"Юманита", "Юнона", "Ядвига", "Яна", "Янина", "Яромира", "Ярослава"
		};

	#endregion

		public RandomNameGenerator()
		{
			_rnd = new Random();
		}

		public static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s)){
				return string.Empty;
			}
			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);
			return new string(a);
		}

		/// <summary>
		/// Переделка фамилии в женскую
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string FemalizeFamily(string s)
		{
			var o = s;// сохраняем переданную строку
			var add = "а";// прибавляем -а для женских фамилий
			var l = s.Length;
			var ls1 = s.Substring(l - 1);
			if (ls1=="й"){// для фамилий заканчивающихся на -ий надо прибавлять -ая 
				o = o.Substring(0, l - 2);
				add = "ая";
			}
			return o + add;
		}


		public string GenerateProjectsName()
		{
			var prefix = color.Union(adjective).ToArray<string>();
			var suffix = science;

			var n1 = (int) (_rnd.NextDouble()*prefix.Length);
			var n1ex = (int) (_rnd.NextDouble()*prefix.Length);
			if (n1ex == n1){
				n1ex = n1 + 1;
			}

			var n2 = (int) (_rnd.NextDouble()*suffix.Length);
			var prename = UppercaseFirst(prefix[n1]);
			var prenameex = UppercaseFirst(prefix[n1ex]);
			var sufname = UppercaseFirst(suffix[n2]);
			var n3 = (int) (_rnd.NextDouble()*100);


			var name = string.Empty;
			if (n3 <= 15){
				name = prenameex + " " + prename + " " + sufname;
			}
			else if (n3 > 15 && n3 <= 30){
				name = sufname + " " + prename;
			}
			else{
				name = prename + " " + sufname;
			}
			return name;
		}

		public string GenerateElfName()
		{
			var name = string.Empty;

			var prefix_male = elf_male;
			var prefix_female = elf_female;
			var suffix = elf_surname;
			var n1m = (int) (_rnd.NextDouble()*prefix_male.Length);
			var n1f = (int) (_rnd.NextDouble()*prefix_female.Length);
			var n2 = (int) (_rnd.NextDouble()*suffix.Length);
			var n2ekstra = (int) (_rnd.NextDouble()*suffix.Length);

			var prename_male = UppercaseFirst(prefix_male[n1m]);
			var prename_female = UppercaseFirst(prefix_female[n1f]);
			var sufname = UppercaseFirst(suffix[n2]);
			var extraname = UppercaseFirst(suffix[n2ekstra]);
			var n3 = (int) (_rnd.NextDouble()*100);
			if (n3 <= 40){
				name = prename_male + " " + sufname;
			}
			else if (n3 > 40 && n3 <= 70){
				name = prename_female + " " + sufname;
			}
			else if (n3 > 70 && n3 <= 85){
				name = prename_male + " " + extraname + " " + sufname;
			}
			else if (n3 > 85){
				name = prename_female + " " + extraname + " " + sufname;
			}

			return name;
		}


		public string GenerateFantasyName()
		{
			var name = string.Empty;

			var prefix_male = fantasy_male;
			var prefix_female = fantasy_female;
			var suffix = fantasy_surname;
			var n1m = (int) (_rnd.NextDouble()*prefix_male.Length);
			var n1f = (int) (_rnd.NextDouble()*prefix_female.Length);
			var n2 = (int) (_rnd.NextDouble()*suffix.Length);
			var n2ekstra = (int) (_rnd.NextDouble()*suffix.Length);
			var prename_male = UppercaseFirst(prefix_male[n1m]);
			var prename_female = UppercaseFirst(prefix_female[n1f]);
			var sufname = UppercaseFirst(suffix[n2]);
			var extraname = UppercaseFirst(suffix[n2ekstra]);
			var n3 = (int) (_rnd.NextDouble()*100);
			if (n3 <= 12){
				name = prename_male;
			}
			else if (n3 > 12 && n3 <= 20){
				name = prename_female;
			}
			else if (n3 > 20 && n3 <= 50){
				name = prename_male + " " + sufname;
			}
			else if (n3 > 50 && n3 <= 70){
				name = prename_female + " " + sufname;
			}
			else if (n3 > 70 && n3 <= 85){
				name = prename_male + " " + extraname + " " + sufname;
			}
			else if (n3 > 85){
				name = prename_female + " " + extraname + " " + sufname;
			}
			return name;
		}

		public string GenerateRusName()
		{
			var name = string.Empty;

			var prefix_male = rus_male;
			var prefix_female = rus_female;
			var suffix = rus_surname;
			var n1m = (int) (_rnd.NextDouble()*prefix_male.Length);
			var n1f = (int) (_rnd.NextDouble()*prefix_female.Length);
			var n2 = (int) (_rnd.NextDouble()*suffix.Length);
			var n2ekstra = (int) (_rnd.NextDouble()*suffix.Length);
			var prename_male = UppercaseFirst(prefix_male[n1m]);
			var prename_female = UppercaseFirst(prefix_female[n1f]);
			var sufname = UppercaseFirst(suffix[n2]);
			var extraname = UppercaseFirst(suffix[n2ekstra]);
			var n3 = (int) (_rnd.NextDouble()*100);
			if (n3 <= 4){
				name = prename_male;
			}
			else if (n3 > 4 && n3 <= 10){
				name = prename_female;
			}
			else if (n3 > 10 && n3 <= 50){
				name = prename_male + " " + sufname;
			}
			else if (n3 > 50 && n3 <= 90){
				name = prename_female + " " + FemalizeFamily(sufname);
			}
			else if (n3 > 90 && n3 <= 95){
				name = prename_male + " " + extraname + " " + sufname;
			}
			else if (n3 > 95){
				name = prename_female + " " + FemalizeFamily(extraname) + " " + FemalizeFamily(sufname);
			}
			return name;
		}


	}
}
