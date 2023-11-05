using System;
using System.Collections.Generic;
using bicycle_store_web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace bicycle_store_web
{
    public partial class bicycle_storeContext : DbContext
    {
        //    public bicycle_storeContext() {}

        public bicycle_storeContext(DbContextOptions<bicycle_storeContext> options) : base(options) { }

        public virtual DbSet<Bicycle> Bicycles { get; set; }
        public virtual DbSet<BicycleOrder> BicycleOrders { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<ShoppingCart> ShopingCarts { get; set; }
        public virtual DbSet<ShoppingCartOrder> ShoppingCartOrders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			int id = 1;
			modelBuilder.Entity<Order>()
			.Property(o => o.OrderId)
			.ValueGeneratedOnAdd();

			modelBuilder.Entity<Type>().HasData(
				new Type { Id = id++, Name = "Mountain", Description = "Mountain bicycle is a bicycle designed for off-road cycling" },
				new Type { Id = id++, Name = "Cross", Description = "Cross bicycle is a specific form of drop-bar bicycle that is built to tackle the popular racing discipline that is cyclocross" },
				new Type { Id = id++, Name = "Road", Description = "Road bicycle is a bicycle designed to take you as far and as fast as your legs can manage on paved surfaces" },
				new Type { Id = id++, Name = "City", Description = "City bicycle is a bicycle used predominantly for trips in urban areas" },
				new Type { Id = id++, Name = "Hybrid", Description = "Hybrid bicycle is a bicycle which combines road bicycle and road bicycle" },
				new Type { Id = id++, Name = "Cruiser", Description = "Cruiser bicycle is a bicycle that usually combines balloon tires, an upright seating posture, a single-speed drivetrain, and straightforward steel construction with expressive styling" }
			);

			id = 1; 
			modelBuilder.Entity<Producer>().HasData(
				new Producer { Id = id++, Name = "Scott", Description = "Scott Sports SA (formerly Scott USA) is a Swiss producer of bicycles, winter equipment, motorsports gear and sportswear..." },
				new Producer { Id = id++, Name = "Merida", Description = "Merida Industry Co., Ltd (MIC; Chinese: 美利達工業) is a Taiwan-based company with R&D headquarters in Germany that designs..." },
				new Producer { Id = id++, Name = "Orbea", Description = "Orbea is a bicycle manufacturer based in Mallabia, Spain. It is part of the Mondragón Cooperative Corporation and Spain's largest bicycle manufacturer..." },
				new Producer { Id = id++, Name = "Dorozhnik", Description = "Dorozhnik is a Ukrainian daughter company of Velotrade. Velotrade is a worldwide famous producer of bikes" },
				new Producer { Id = id++, Name = "Bergamont", Description = "Bergamont is a famous German producer of bicycles" }
			);

			id = 1; 
			modelBuilder.Entity<Country>().HasData(
				new Country { Id = id++, Name = "Afghanistan" },
				new Country { Id = id++, Name = "Albania" },
				new Country { Id = id++, Name = "Algeria" },
				new Country { Id = id++, Name = "American Samoa" },
				new Country { Id = id++, Name = "Andorra" },
				new Country { Id = id++, Name = "Angola" },
				new Country { Id = id++, Name = "Anguilla" },
				new Country { Id = id++, Name = "Antarctica" },
				new Country { Id = id++, Name = "Antigua And Barbuda" },
				new Country { Id = id++, Name = "Argentina" },
				new Country { Id = id++, Name = "Armenia" },
				new Country { Id = id++, Name = "Aruba" },
				new Country { Id = id++, Name = "Australia" },
				new Country { Id = id++, Name = "Austria" },
				new Country { Id = id++, Name = "Azerbaijan" },
				new Country { Id = id++, Name = "Bahamas The" },
				new Country { Id = id++, Name = "Bahrain" },
				new Country { Id = id++, Name = "Bangladesh" },
				new Country { Id = id++, Name = "Barbados" },
				new Country { Id = id++, Name = "Belarus" },
				new Country { Id = id++, Name = "Belgium" },
				new Country { Id = id++, Name = "Belize" },
				new Country { Id = id++, Name = "Benin" },
				new Country { Id = id++, Name = "Bermuda" },
				new Country { Id = id++, Name = "Bhutan" },
				new Country { Id = id++, Name = "Bolivia" },
				new Country { Id = id++, Name = "Bosnia and Herzegovina" },
				new Country { Id = id++, Name = "Botswana" },
				new Country { Id = id++, Name = "Bouvet Island" },
				new Country { Id = id++, Name = "Brazil" },
				new Country { Id = id++, Name = "British Indian Ocean Territory" },
				new Country { Id = id++, Name = "Brunei" },
				new Country { Id = id++, Name = "Bulgaria" },
				new Country { Id = id++, Name = "Burkina Faso" },
				new Country { Id = id++, Name = "Burundi" },
				new Country { Id = id++, Name = "Cambodia" },
				new Country { Id = id++, Name = "Cameroon" },
				new Country { Id = id++, Name = "Canada" },
				new Country { Id = id++, Name = "Cape Verde" },
				new Country { Id = id++, Name = "Cayman Islands" },
				new Country { Id = id++, Name = "Central African Republic" },
				new Country { Id = id++, Name = "Chad" },
				new Country { Id = id++, Name = "Chile" },
				new Country { Id = id++, Name = "China" },
				new Country { Id = id++, Name = "Christmas Island" },
				new Country { Id = id++, Name = "Cocos (Keeling) Islands" },
				new Country { Id = id++, Name = "Colombia" },
				new Country { Id = id++, Name = "Comoros" },
				new Country { Id = id++, Name = "Congo" },
				new Country { Id = id++, Name = "Congo The Democratic Republic Of The" },
				new Country { Id = id++, Name = "Cook Islands" },
				new Country { Id = id++, Name = "Costa Rica" },
				new Country { Id = id++, Name = "Ivory Coast" },
				new Country { Id = id++, Name = "Croatia (Hrvatska)" },
				new Country { Id = id++, Name = "Cuba" },
				new Country { Id = id++, Name = "Cyprus" },
				new Country { Id = id++, Name = "Czech Republic" },
				new Country { Id = id++, Name = "Denmark" },
				new Country { Id = id++, Name = "Djibouti" },
				new Country { Id = id++, Name = "Dominica" },
				new Country { Id = id++, Name = "Dominican Republic" },
				new Country { Id = id++, Name = "East Timor" },
				new Country { Id = id++, Name = "Ecuador" },
				new Country { Id = id++, Name = "Egypt" },
				new Country { Id = id++, Name = "El Salvador" },
				new Country { Id = id++, Name = "Equatorial Guinea" },
				new Country { Id = id++, Name = "Eritrea" },
				new Country { Id = id++, Name = "Estonia" },
				new Country { Id = id++, Name = "Ethiopia" },
				new Country { Id = id++, Name = "External Territories of Australia" },
				new Country { Id = id++, Name = "Falkland Islands" },
				new Country { Id = id++, Name = "Faroe Islands" },
				new Country { Id = id++, Name = "Fiji Islands" },
				new Country { Id = id++, Name = "Finland" },
				new Country { Id = id++, Name = "France" },
				new Country { Id = id++, Name = "French Guiana" },
				new Country { Id = id++, Name = "French Polynesia" },
				new Country { Id = id++, Name = "French Southern Territories" },
				new Country { Id = id++, Name = "Gabon" },
				new Country { Id = id++, Name = "Gambia The" },
				new Country { Id = id++, Name = "Georgia" },
				new Country { Id = id++, Name = "Germany" },
				new Country { Id = id++, Name = "Ghana" },
				new Country { Id = id++, Name = "Gibraltar" },
				new Country { Id = id++, Name = "Greece" },
				new Country { Id = id++, Name = "Greenland" },
				new Country { Id = id++, Name = "Grenada" },
				new Country { Id = id++, Name = "Guadeloupe" },
				new Country { Id = id++, Name = "Guam" },
				new Country { Id = id++, Name = "Guatemala" },
				new Country { Id = id++, Name = "Guernsey and Alderney" },
				new Country { Id = id++, Name = "Guinea" },
				new Country { Id = id++, Name = "Guinea-Bissau" },
				new Country { Id = id++, Name = "Guyana" },
				new Country { Id = id++, Name = "Haiti" },
				new Country { Id = id++, Name = "Heard and McDonald Islands" },
				new Country { Id = id++, Name = "Honduras" },
				new Country { Id = id++, Name = "Hong Kong S.A.R." },
				new Country { Id = id++, Name = "Hungary" },
				new Country { Id = id++, Name = "Iceland" },
				new Country { Id = id++, Name = "India" },
				new Country { Id = id++, Name = "Indonesia" },
				new Country { Id = id++, Name = "Iran" },
				new Country { Id = id++, Name = "Iraq" },
				new Country { Id = id++, Name = "Ireland" },
				new Country { Id = id++, Name = "Israel" },
				new Country { Id = id++, Name = "Italy" },
				new Country { Id = id++, Name = "Jamaica" },
				new Country { Id = id++, Name = "Japan" },
				new Country { Id = id++, Name = "Jersey" },
				new Country { Id = id++, Name = "Jordan" },
				new Country { Id = id++, Name = "Kazakhstan" },
				new Country { Id = id++, Name = "Kenya" },
				new Country { Id = id++, Name = "Kiribati" },
				new Country { Id = id++, Name = "Korea North" },
				new Country { Id = id++, Name = "Korea South" },
				new Country { Id = id++, Name = "Kuwait" },
				new Country { Id = id++, Name = "Kyrgyzstan" },
				new Country { Id = id++, Name = "Laos" },
				new Country { Id = id++, Name = "Latvia" },
				new Country { Id = id++, Name = "Lebanon" },
				new Country { Id = id++, Name = "Lesotho" },
				new Country { Id = id++, Name = "Liberia" },
				new Country { Id = id++, Name = "Libya" },
				new Country { Id = id++, Name = "Liechtenstein" },
				new Country { Id = id++, Name = "Lithuania" },
				new Country { Id = id++, Name = "Luxembourg" },
				new Country { Id = id++, Name = "Macau S.A.R." },
				new Country { Id = id++, Name = "Macedonia" },
				new Country { Id = id++, Name = "Madagascar" },
				new Country { Id = id++, Name = "Malawi" },
				new Country { Id = id++, Name = "Malaysia" },
				new Country { Id = id++, Name = "Maldives" },
				new Country { Id = id++, Name = "Mali" },
				new Country { Id = id++, Name = "Malta" },
				new Country { Id = id++, Name = "Man (Isle of)" },
				new Country { Id = id++, Name = "Marshall Islands" },
				new Country { Id = id++, Name = "Martinique" },
				new Country { Id = id++, Name = "Mauritania" },
				new Country { Id = id++, Name = "Mauritius" },
				new Country { Id = id++, Name = "Mayotte" },
				new Country { Id = id++, Name = "Mexico" },
				new Country { Id = id++, Name = "Micronesia" },
				new Country { Id = id++, Name = "Moldova" },
				new Country { Id = id++, Name = "Monaco" },
				new Country { Id = id++, Name = "Mongolia" },
				new Country { Id = id++, Name = "Montserrat" },
				new Country { Id = id++, Name = "Morocco" },
				new Country { Id = id++, Name = "Mozambique" },
				new Country { Id = id++, Name = "Myanmar" },
				new Country { Id = id++, Name = "Namibia" },
				new Country { Id = id++, Name = "Nauru" },
				new Country { Id = id++, Name = "Nepal" },
				new Country { Id = id++, Name = "Netherlands Antilles" },
				new Country { Id = id++, Name = "Netherlands The" },
				new Country { Id = id++, Name = "New Caledonia" },
				new Country { Id = id++, Name = "New Zealand" },
				new Country { Id = id++, Name = "Nicaragua" },
				new Country { Id = id++, Name = "Niger" },
				new Country { Id = id++, Name = "Nigeria" },
				new Country { Id = id++, Name = "Niue" },
				new Country { Id = id++, Name = "Norfolk Island" },
				new Country { Id = id++, Name = "Northern Mariana Islands" },
				new Country { Id = id++, Name = "Norway" },
				new Country { Id = id++, Name = "Oman" },
				new Country { Id = id++, Name = "Pakistan" },
				new Country { Id = id++, Name = "Palau" },
				new Country { Id = id++, Name = "Palestinian Territory Occupied" },
				new Country { Id = id++, Name = "Panama" },
				new Country { Id = id++, Name = "Papua new Guinea" },
				new Country { Id = id++, Name = "Paraguay" },
				new Country { Id = id++, Name = "Peru" },
				new Country { Id = id++, Name = "Philippines" },
				new Country { Id = id++, Name = "Pitcairn Island" },
				new Country { Id = id++, Name = "Poland" },
				new Country { Id = id++, Name = "Portugal" },
				new Country { Id = id++, Name = "Puerto Rico" },
				new Country { Id = id++, Name = "Qatar" },
				new Country { Id = id++, Name = "Reunion" },
				new Country { Id = id++, Name = "Romania" },
				new Country { Id = id++, Name = "Rwanda" },
				new Country { Id = id++, Name = "Saint Helena" },
				new Country { Id = id++, Name = "Saint Kitts And Nevis" },
				new Country { Id = id++, Name = "Saint Lucia" },
				new Country { Id = id++, Name = "Saint Pierre and Miquelon" },
				new Country { Id = id++, Name = "Saint Vincent And The Grenadines" },
				new Country { Id = id++, Name = "Samoa" },
				new Country { Id = id++, Name = "San Marino" },
				new Country { Id = id++, Name = "Sao Tome and Principe" },
				new Country { Id = id++, Name = "Saudi Arabia" },
				new Country { Id = id++, Name = "Senegal" },
				new Country { Id = id++, Name = "Serbia" },
				new Country { Id = id++, Name = "Seychelles" },
				new Country { Id = id++, Name = "Sierra Leone" },
				new Country { Id = id++, Name = "Singapore" },
				new Country { Id = id++, Name = "Slovakia" },
				new Country { Id = id++, Name = "Slovenia" },
				new Country { Id = id++, Name = "Smaller Territories of the UK" },
				new Country { Id = id++, Name = "Solomon Islands" },
				new Country { Id = id++, Name = "Somalia" },
				new Country { Id = id++, Name = "South Africa" },
				new Country { Id = id++, Name = "South Georgia" },
				new Country { Id = id++, Name = "South Sudan" },
				new Country { Id = id++, Name = "Spain" },
				new Country { Id = id++, Name = "Sri Lanka" },
				new Country { Id = id++, Name = "Sudan" },
				new Country { Id = id++, Name = "Suriname" },
				new Country { Id = id++, Name = "Svalbard And Jan Mayen Islands" },
				new Country { Id = id++, Name = "Swaziland" },
				new Country { Id = id++, Name = "Sweden" },
				new Country { Id = id++, Name = "Switzerland" },
				new Country { Id = id++, Name = "Syria" },
				new Country { Id = id++, Name = "Taiwan" },
				new Country { Id = id++, Name = "Tajikistan" },
				new Country { Id = id++, Name = "Tanzania" },
				new Country { Id = id++, Name = "Thailand" },
				new Country { Id = id++, Name = "Togo" },
				new Country { Id = id++, Name = "Tokelau" },
				new Country { Id = id++, Name = "Tonga" },
				new Country { Id = id++, Name = "Trinidad And Tobago" },
				new Country { Id = id++, Name = "Tunisia" },
				new Country { Id = id++, Name = "Turkey" },
				new Country { Id = id++, Name = "Turkmenistan" },
				new Country { Id = id++, Name = "Turks And Caicos Islands" },
				new Country { Id = id++, Name = "Tuvalu" },
				new Country { Id = id++, Name = "Uganda" },
				new Country { Id = id++, Name = "Ukraine" },
				new Country { Id = id++, Name = "United Arab Emirates" },
				new Country { Id = id++, Name = "United Kingdom" },
				new Country { Id = id++, Name = "United States" },
				new Country { Id = id++, Name = "United States Minor Outlying Islands" },
				new Country { Id = id++, Name = "Uruguay" },
				new Country { Id = id++, Name = "Uzbekistan" },
				new Country { Id = id++, Name = "Vanuatu" },
				new Country { Id = id++, Name = "Vatican City State (Holy See)" },
				new Country { Id = id++, Name = "Venezuela" },
				new Country { Id = id++, Name = "Vietnam" },
				new Country { Id = id++, Name = "Virgin Islands (British)" },
				new Country { Id = id++, Name = "Virgin Islands (US)" },
				new Country { Id = id++, Name = "Wallis And Futuna Islands" },
				new Country { Id = id++, Name = "Western Sahara" },
				new Country { Id = id++, Name = "Yemen" },
				new Country { Id = id++, Name = "Yugoslavia" },
				new Country { Id = id++, Name = "Zambia" },
				new Country { Id = id++, Name = "Zimbabwe" }
			);

			id = 1;
			byte[] bicyclePhotoBytes = System.IO.File.ReadAllBytes("C:\\Projects\\bicycle_store_web\\bicycle_store_web\\Resources\\bicycle.png");
			modelBuilder.Entity<Bicycle>().HasData(
				new Bicycle { Id = id++, Name = "Contessa Active 60", WheelDiameter = 29, Price = 16800, Quantity = 5, TypeId = 1, ProducerId = 1, Photo = bicyclePhotoBytes, CountryId = 1 },
				new Bicycle { Id = id++, Name = "Aspect 970", WheelDiameter = 29, Price = 16975, Quantity = 5, TypeId = 1, ProducerId = 1, Photo = bicyclePhotoBytes, CountryId = 1 },
				new Bicycle { Id = id++, Name = "Sub Cross 50", WheelDiameter = 28, Price = 18375, Quantity = 5, TypeId = 2, ProducerId = 1, Photo = bicyclePhotoBytes, CountryId = 1 },
				new Bicycle { Id = id++, Name = "Roxter 26", WheelDiameter = 26, Price = 18375, Quantity = 5, TypeId = 2, ProducerId = 1, Photo = bicyclePhotoBytes, CountryId = 1 },
				new Bicycle { Id = id++, Name = "Corossway 40", WheelDiameter = 28, Price = 18994, Quantity = 5, TypeId = 2, ProducerId = 2, Photo = bicyclePhotoBytes, CountryId = 2 },
				new Bicycle { Id = id++, Name = "Crossway 20-D", WheelDiameter = 28, Price = 17109, Quantity = 5, TypeId = 2, ProducerId = 2, Photo = bicyclePhotoBytes, CountryId = 2 },
				new Bicycle { Id = id++, Name = "Matts 7.20", WheelDiameter = 27.5f, Price = 16877, Quantity = 5, TypeId = 1, ProducerId = 2, Photo = bicyclePhotoBytes, CountryId = 2 },
				new Bicycle { Id = id++, Name = "Matts 7.30", WheelDiameter = 27.5f, Price = 18414, Quantity = 5, TypeId = 1, ProducerId = 2, Photo = bicyclePhotoBytes, CountryId = 2 },
				new Bicycle { Id = id++, Name = "MX 27 ENT DIRT", WheelDiameter = 26, Price = 17645, Quantity = 5, TypeId = 3, ProducerId = 3, Photo = bicyclePhotoBytes, CountryId = 3 },
				new Bicycle { Id = id++, Name = "MX 50", WheelDiameter = 29, Price = 18393, Quantity = 5, TypeId = 1, ProducerId = 3, Photo = bicyclePhotoBytes, CountryId = 3 },
				new Bicycle { Id = id++, Name = "MX 24 XC", WheelDiameter = 24, Price = 18107, Quantity = 5, TypeId = 1, ProducerId = 3, Photo = bicyclePhotoBytes, CountryId = 3 },
				new Bicycle { Id = id++, Name = "MX 20 Team", WheelDiameter = 20, Price = 14593, Quantity = 5, TypeId = 1, ProducerId = 3, Photo = bicyclePhotoBytes, CountryId = 3 },
				new Bicycle { Id = id++, Name = "Lux", WheelDiameter = 26, Price = 8451, Quantity = 5, TypeId = 4, ProducerId = 4, Photo = bicyclePhotoBytes, CountryId = 4 },
				new Bicycle { Id = id++, Name = "Comfort", WheelDiameter = 28, Price = 5941, Quantity = 5, TypeId = 5, ProducerId = 4, Photo = bicyclePhotoBytes, CountryId = 4 },
				new Bicycle { Id = id++, Name = "Topaz", WheelDiameter = 28, Price = 4851, Quantity = 5, TypeId = 6, ProducerId = 4, Photo = bicyclePhotoBytes, CountryId = 4 },
				new Bicycle { Id = id++, Name = "Retro Planetary Hub", WheelDiameter = 28, Price = 7543, Quantity = 5, TypeId = 6, ProducerId = 4, Photo = bicyclePhotoBytes, CountryId = 4 },
				new Bicycle { Id = id++, Name = "Revox", WheelDiameter = 24, Price = 9900, Quantity = 5, TypeId = 1, ProducerId = 5, Photo = bicyclePhotoBytes, CountryId = 5 },
				new Bicycle { Id = id++, Name = "Revox 2", WheelDiameter = 27.5f, Price = 14175, Quantity = 5, TypeId = 1, ProducerId = 5, Photo = bicyclePhotoBytes, CountryId = 5 },
				new Bicycle { Id = id++, Name = "Revox 3", WheelDiameter = 27.5f, Price = 17010, Quantity = 5, TypeId = 1, ProducerId = 5, Photo = bicyclePhotoBytes, CountryId = 5 },
				new Bicycle { Id = id++, Name = "Revox 4", WheelDiameter = 29, Price = 18833, Quantity = 5, TypeId = 1, ProducerId = 5, Photo = bicyclePhotoBytes, CountryId = 5 }
			);

			id = 1;
			byte[] userPhotoBytes = System.IO.File.ReadAllBytes("C:\\Projects\\bicycle_store_web\\bicycle_store_web\\Resources\\bicycle.png");
			byte[] adminPhotoBytes = System.IO.File.ReadAllBytes("C:\\Projects\\bicycle_store_web\\bicycle_store_web\\Resources\\bicycle.png");

			modelBuilder.Entity<User>().HasData(
				new User { Id = id++, FullName = "superAdmin", Phone = "1111", Email = "super_admin@gmail.com", Adress = "adress", Username = "superAdmin", Password = "superAdmin", Role = "SuperAdmin", Photo = adminPhotoBytes },
				new User { Id = id++, FullName = "admin", Phone = "1111", Email = "admin@gmail.com", Adress = "adress", Username = "admin", Password = "admin", Role = "Admin", Photo = adminPhotoBytes },
				new User { Id = id++, FullName = "user", Phone = "1111", Email = "user@gmail.com", Adress = "adress", Username = "user", Password = "user", Role = "User", Photo = userPhotoBytes }
			);

			id = 1; 
			modelBuilder.Entity<ShoppingCart>().HasData(
				new ShoppingCart { Id = id++, UserId = 1 },
				new ShoppingCart { Id = id++, UserId = 2 },
				new ShoppingCart { Id = id++, UserId = 3 }
			);
		}
	}
}
