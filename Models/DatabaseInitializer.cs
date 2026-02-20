using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Initilal_YV_Assesment2.Models
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<MefistoDbContext>
    {
        protected override void Seed(MefistoDbContext context)
        {
            base.Seed(context);
            if (!context.Users.Any())
            {
                //create a few roles and store them in AspNetRoles table

                //create a roleManeger object will allow us to create roles and store them in the database
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //if the Admin role doesn't exist
                if (!roleManager.RoleExists("Admin"))
                {
                    //create an Admin role
                    roleManager.Create(new IdentityRole("Admin"));
                }

                //if the Member role doesn't exist
                if (!roleManager.RoleExists("Member"))
                {
                    //create a Member role
                    roleManager.Create(new IdentityRole("Member"));
                }

                //if the Member role doesn't exist
                if (!roleManager.RoleExists("Staff"))
                {
                    //create a Member role
                    roleManager.Create(new IdentityRole("Staff"));
                }
                //save new roles to the database
                context.SaveChanges();


                //********************************************************
                //create some users now and assign them to different toles
                //********************************************************

                //the userManager object allows creating users and store them in the database
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                
                //create a user Admin
                var admin = new User()
                {
                    BecomeEmployee = new DateTime(2019, 1, 6),
                    UserName = "theBestAdminEver@mefistotheatre.com",
                    Email = "theBestAdminEver@mefistotheatre.com",
                    FirstName = "Jim",
                    SecondName = "Smith",
                    AddressLine1 = "56 High Street",
                    City = "Glasgow",
                    Postcode = "G1 67AD",
                    EmailConfirmed = true,
                    PhoneNumber = "07799112233",
                    RegisteredAt = new DateTime(2018, 9, 8)
                };
                //if users with admin@splittree.com username doesn't exist then ->)

                if (userManager.FindByName("theBestAdminEver@mefistotheatre.com") == null)
                {
                    //super relaxed password validator
                    userManager.PasswordValidator = new PasswordValidator()
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false,
                        RequireUppercase = false
                    };

                    //add the hashed password to user
                    userManager.Create(admin, "admin123");

                    //add the user to the role Admin
                    userManager.AddToRole(admin.Id, "Admin");
                }

                //create an employee
                var staff = new User()
                {
                    BecomeEmployee = new DateTime(2019, 1, 10),
                    UserName = "staff@mefistotheatre.com",
                    Email = "staff@mefistotheatre.com",
                    FirstName = "Paul",
                    SecondName = "Goat",
                    AddressLine1 = "5 Meerry Street",
                    AddressLine2 = "Flat 6",
                    City = "Glasgow",
                    Postcode = "G5 7AD",
                    EmailConfirmed = true,
                    PhoneNumber = "02244772233",
                    RegisteredAt = new DateTime(2018, 9, 8)
                };

                if (userManager.FindByName("staff@mefistotheatre.com") == null)
                {
                    //super relaxed password validator
                    userManager.PasswordValidator = new PasswordValidator()
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false,
                        RequireUppercase = false
                    };
                    //add the hashed password to user
                    userManager.Create(staff, "password1");
                    //add the user to the role "Staff"
                    userManager.AddToRole(staff.Id, "Staff");
                }

                //create a member 
                var member = new User()
                {
                    UserName = "member@mefistotheatre.com",
                    Email = "member@mefistotheatre.com",
                    FirstName = "Luigi",
                    SecondName = "Monk",
                    AddressLine1 = "34 Confused Street",
                    City = "Edinburg",
                    Postcode = "8P9 7Y8",
                    IsSuspended = false,
                    EmailConfirmed = true,
                    PhoneNumber = "05588112233",
                    RegisteredAt = new DateTime(2023, 9, 8, 8, 0, 15)
                };
                if (userManager.FindByName("member@mefistotheatre.com") == null)
                {
                    //super relaxed password validator
                    userManager.PasswordValidator = new PasswordValidator()
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false,
                        RequireUppercase = false
                    };

                        ////add the hashed password to user
                        //userManager.Create(member, "password1");
                        ////add the user to the role "Member"
                        //userManager.AddToRole(member.Id, "Member");

                        ////add to the member table
                        //context.Members.Add(member);
                        var result = userManager.Create(member, "Password1!");

                        if (!result.Succeeded)
                        {
                            // SEE REAL ERRORS
                            foreach (var err in result.Errors)
                            {
                                Debug.WriteLine(err);
                            }
                            return;
                        }

                        userManager.AddToRole(member.Id, "Member");
                    }

                //create a suspended member 
                var member1 = new User()
                {
                    UserName = "member2@mefistotheatre.com",
                    Email = "member2@mefistotheatre.com",
                    FirstName = "Monica",
                    SecondName = "Beluchi",
                    AddressLine1 = "3 Halfords Street",
                    City = "Aberdeen",
                    Postcode = "9IH 78Y",
                    IsSuspended = true,
                    EmailConfirmed = true,
                    PhoneNumber = "05588112233",
                    RegisteredAt = new DateTime(2022, 3, 25, 8, 0, 15),
                    SuspendedUntil = new DateTime(2022, 4, 25, 8, 0, 15)
                };

                if (userManager.FindByName("member2@mefistotheatre.com") == null)
                {
                    //super relaxed password validator
                    userManager.PasswordValidator = new PasswordValidator()
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false,
                        RequireUppercase = false
                    };
                        var result = userManager.Create(member1, "Password1!");

                        if (!result.Succeeded)
                        {
                            // SEE REAL ERRORS
                            foreach (var err in result.Errors)
                            {
                                Debug.WriteLine(err);
                            }
                            return;
                        }

                        userManager.AddToRole(member1.Id, "Member"); ;

                }

                //save changes to the database
                context.SaveChanges();

                //**********************************
                //seeding the Categories table
                //**********************************

                //creating a few categories
                var cat1 = new Category() { Name = "Announcements" };
                var cat2 = new Category() { Name = "Movie Reviews" };
                var cat3 = new Category() { Name = "Performance Reviews" };
                var cat4 = new Category() { Name = "Behind the Scenes" };
                var cat5 = new Category() { Name = "Community News" };

                //add each category to the Categories table
                context.Categories.Add(cat1);
                context.Categories.Add(cat2);
                context.Categories.Add(cat3);
                context.Categories.Add(cat4);
                context.Categories.Add(cat5);

                //save changes to the database
                context.SaveChanges();

                //*******************************
                //seeding the post table
                //*******************************

                var post1 = new Post()
                {
                    Title = "Mefisto is Going Green: Digital Programs Only!",
                    Content = "In our effort to be more sustainable and reach our younger, " +
                    "tech-savvy audience, we are moving to 100% digital programs. Simply scan " +
                    "the QR code on the back of your seat to see the cast bios and show credits. " +
                    "Not only does this save trees, but it also allows us to include exclusive \"Director's Cut\" " +
                    "videos and rehearsal footage right on your phone!",
                    DatePosted = new DateTime(2019, 1, 1, 9, 0, 15),
                    DateEdited= new DateTime(2019, 1, 1, 9, 0, 15).AddDays(14),
                    User = staff,
                    Category = cat1
                };
                context.Posts.Add(post1);

                var post2 = new Post()
                {
                    Title = "Retro Night: Why \"Metropolis\" Still Matters",
                    Content = "Last night’s screening of the 1927 classic Metropolis proved that great " +
                    "cinema is timeless. Even a century later, the visual effects and social commentary felt " +
                    "incredibly relevant. It was wonderful to see so many students in the audience - our student " +
                    "discount nights are clearly working! Join the Discussion: What is your favorite silent-era film?" +
                    " Leave a comment below and let us know what we should screen next month!",
                    DatePosted = new DateTime(2019, 1, 19),
                    DateEdited = new DateTime(2019, 1, 19),
                    User = admin,
                    Category = cat2
                };
                context.Posts.Add(post2);

                var post3 = new Post()
                {
                    Title = "Opening Night: \"Shadows of the Moor\"",
                    Content = "A haunting, visceral, and breathtaking performance. The lead actress captured the" +
                    " desperation of the moors perfectly. The minimalist set design (which is very lightweight and " +
                    "easy to move!) allowed the acting to take center stage. Moderation Reminder: We welcome all opinions, " +
                    "but please keep reviews constructive. Inappropriate language will be removed by the Mefisto moderation team.",
                    DatePosted = new DateTime(2020, 12, 7), 
                    DateEdited = new DateTime(2020, 12, 7),
                    User = admin,
                    Category = cat3
                };
                context.Posts.Add(post3);

                var post4 = new Post()
                {
                    Title = "Meet the Tech Crew: The Magic of Projection Mapping",
                    Content = "Ever wonder how we turned a wooden floor into a" +
                    " rushing river for The Tempest? Meet our lead technician, David! " +
                    "He uses open-source software to map high-definition visuals onto our stage. " +
                    "This approach keeps our production costs low while delivering big budget " +
                    "visuals that look amazing from every angle in the house.",
                    DatePosted = new DateTime(2021, 1, 25),
                    DateEdited = new DateTime(2021, 1, 25),
                    User = staff,
                    Category = cat4
                };
                context.Posts.Add(post4);

                //save the changes to the database
                context.SaveChanges();

                //*******************************
                //seeding the comments table
                //*******************************

                var comment1 = new Comment()
                {
                    CommentText = "Absolutely stunning performance! " +
                    "I’ve seen three different versions of this play over the years, " +
                    "but the way Mefisto used the lighting to create that eerie moor" +
                    " atmosphere was unlike anything else. My phone's digital program worked" +
                    " perfectly, too—it was great to read the actor bios during intermission without fumbling with paper!",
                    DatePosted = new DateTime(2020, 12, 20),
                    Post = post3,
                    User = member
                };
                context.Comments.Add(comment1);


                var comment2 = new Comment()
                {
                    CommentText = "This digital program idea is absolute rubbish!! " +
                    "Not everyone wants to stare at a screen. You guys are just being lazy and cheap. " +
                    "[Insert inappropriate insults/spams here]. I'm never coming back to this theatre again!",
                    DatePosted = new DateTime(2019, 1, 15),
                    Post = post1,
                    User = member1
                };
                context.Comments.Add(comment2);

                var comment3 = new Comment()
                {
                    CommentText = "This is so cool! I’m studying media design " +
                    "and it’s inspiring to see open-source tools being used in a " +
                    "professional theatre setting. Is there any chance the Mefisto " +
                    "crew will be hosting a 'tech-only' tour or a workshop for students " +
                    "to see the software in action behind the scenes?",
                    DatePosted = new DateTime(2021, 2, 15),
                    Post = post4,
                    User = member
                };
                context.Comments.Add(comment3);

                //save the changes to the database
                context.SaveChanges();
                
            }
        }//end of a seed method
    }// end of a class
}//end of a namespace