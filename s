[33mcommit 43b3db1b8f1f690a92835073250173584f2d53e2[m[33m ([m[1;36mHEAD[m[33m -> [m[1;32mmain[m[33m, [m[1;31morigin/main[m[33m, [m[1;31morigin/HEAD[m[33m)[m
Author: Tengano <blue.dragon.2k5@gmail.com>
Date:   Wed Oct 15 10:20:44 2025 +0700

    Sửa lại phần trang chủ, xoá bớt comment

 Controllers/AccountController.cs |   8 [31m-[m
 Controllers/AdminController.cs   |  23 [32m+[m[31m--[m
 Controllers/HomeController.cs    |   2 [31m-[m
 Controllers/JobsController.cs    |   7 [32m+[m[31m-[m
 Data/ApplicationDbContext.cs     |  12 [31m--[m
 Models/Application.cs            |   1 [31m-[m
 Program.cs                       |   5 [31m-[m
 Views/Account/Login.cshtml       |  12 [32m+[m[31m-[m
 Views/Account/Register.cshtml    |   2 [32m+[m[31m-[m
 Views/Home/Index.cshtml          |  15 [32m+[m[31m-[m
 Views/Shared/_Layout.cshtml      |  65 [32m+[m[31m------[m
 wwwroot/css/modern-auth.css      | 367 [32m++++++++++++++++++++++++++++++[m[31m---------[m
 wwwroot/css/modern-footer.css    | 324 [32m+++++[m[31m-----------------------------[m
 wwwroot/css/style.css            |  25 [32m+[m[31m--[m
 wwwroot/js/admin.js              |  10 [32m+[m[31m-[m
 15 files changed, 343 insertions(+), 535 deletions(-)

[33mcommit b1f3bf27b730537a54581c8c56bf26d1ac7e58e5[m
Author: Tengano <blue.dragon.2k5@gmail.com>
Date:   Sun Oct 12 22:38:00 2025 +0700

    Sửa lại trang chủ (Gần hoàn chỉnh)

 CompleteDatabase.sql          | 611 [31m------------------------------------------[m
 Views/Account/Login.cshtml    | 107 [32m++++++[m[31m--[m
 Views/Account/Register.cshtml | 174 [32m+++++++++[m[31m---[m
 Views/Home/About.cshtml       |   1 [32m+[m
 Views/Home/Index.cshtml       | 106 [31m--------[m
 Views/Shared/_Layout.cshtml   | 165 [32m++++++[m[31m------[m
 wwwroot/css/animations.css    | 375 [31m--------------------------[m
 wwwroot/css/modern-auth.css   | 561 [32m++++++++++++++++++++++++++++++++++++++[m
 wwwroot/css/modern-footer.css | 342 [32m+++++++++++++++++++++++[m
 wwwroot/css/style.css         |  94 [32m+[m[31m------[m
 wwwroot/images/README.md      |  70 [31m-----[m
 wwwroot/js/animations.js      |  90 [31m-------[m
 12 files changed, 1205 insertions(+), 1491 deletions(-)

[33mcommit 377efb6fa00cb0cd04e3a0069efe90b34648a416[m
Author: Tengano <blue.dragon.2k5@gmail.com>
Date:   Wed Oct 8 08:31:40 2025 +0700

    Cập nhật thêm bảng Liên Hệ

 CompleteDatabase.sql             | 307 [32m+++++++++++++++++++++++++++++++[m[31m--------[m
 Controllers/AdminController.cs   |   6 [32m+[m[31m-[m
 Controllers/HomeController.cs    |  33 [32m++++[m[31m-[m
 Data/ApplicationDbContext.cs     |  17 [32m+++[m
 Models/Contact.cs                |  34 [32m+++++[m
 Views/Account/Profile.cshtml     |   2 [32m+[m[31m-[m
 Views/Home/Contact.cshtml        |  27 [32m++[m[31m--[m
 Views/Shared/_AdminLayout.cshtml |   2 [32m+[m[31m-[m
 Views/Shared/_Layout.cshtml      |  19 [32m+[m[31m--[m
 9 files changed, 358 insertions(+), 89 deletions(-)

[33mcommit f82d9fe606badf456d391810781e697c9a195cb6[m
Author: Tengano <blue.dragon.2k5@gmail.com>
Date:   Tue Oct 7 16:42:59 2025 +0700

    Cập nhật thêm animation

 Controllers/AccountController.cs  |   56 [32m+[m[31m-[m
 Controllers/AdminController.cs    |  100 [32m++[m[31m-[m
 Controllers/HomeController.cs     |    5 [32m+[m
 Controllers/JobsController.cs     |    6 [32m+[m[31m-[m
 Data/ApplicationDbContext.cs      |    2 [32m+[m[31m-[m
 Models/Application.cs             |    2 [32m+[m[31m-[m
 Models/Job.cs                     |    3 [32m+[m[31m-[m
 Models/User.cs                    |    2 [32m+[m[31m-[m
 Services/AuthenticationService.cs |    2 [31m-[m
 Views/Account/Profile.cshtml      |  445 [32m++++++++++++[m
 Views/Admin/Index.cshtml          |  246 [32m++++[m[31m---[m
 Views/Home/About.cshtml           |    6 [32m+[m[31m-[m
 Views/Home/Contact.cshtml         |   16 [32m+[m[31m-[m
 Views/Home/Index.cshtml           |  527 [32m+++++++++++[m[31m---[m
 Views/Shared/_AdminLayout.cshtml  |  140 [32m+++[m[31m-[m
 Views/Shared/_Layout.cshtml       |  136 [32m+++[m[31m-[m
 Views/Shared/_LoginPartial.cshtml |   11 [32m+[m[31m-[m
 wwwroot/css/admin.css             |  904 [32m++++++++++++++++++++++++[m
 wwwroot/css/animations.css        |  375 [32m++++++++++[m
 wwwroot/css/style.css             | 1405 [32m+++++++++++++++++++++++++++++++++++++[m
 wwwroot/images/README.md          |   70 [32m++[m
 wwwroot/js/admin.js               |   10 [32m+[m[31m-[m
 wwwroot/js/animations.js          |   90 [32m+++[m
 23 files changed, 4270 insertions(+), 289 deletions(-)

[33mcommit 9cbbcf43dfa33904e2b172d147ba988cceb2e486[m
Author: Tengano <blue.dragon.2k5@gmail.com>
Date:   Mon Oct 6 15:09:44 2025 +0700

    Thay đổi một số thuộc tính

 CompleteDatabase.sql                  | 231 [32m++++++++++++++++++++++++[m[31m----------[m
 Controllers/AdminController.cs        |  37 [32m+++[m[31m---[m
 Controllers/HomeController.cs         |   9 [32m+[m[31m-[m
 Controllers/JobsController.cs         |  39 [32m+++[m[31m---[m
 Data/ApplicationDbContext.cs          | 187 [32m++++++++++[m[31m-----------------[m
 Models/Application.cs                 |  31 [32m+++[m[31m--[m
 Models/Candidate.cs                   |  23 [32m++[m[31m--[m
 Models/Job.cs                         |  37 [32m+++[m[31m---[m
 Models/User.cs                        |  30 [32m+++[m[31m--[m
 Views/Admin/ApplicationDetails.cshtml |  56 [32m++++[m[31m-----[m
 Views/Admin/Applications.cshtml       |  22 [32m++[m[31m--[m
 Views/Admin/Candidates.cshtml         |   8 [32m+[m[31m-[m
 Views/Admin/CreateJob.cshtml          |  86 [32m++++++[m[31m-------[m
 Views/Admin/DeleteJob.cshtml          |  10 [32m+[m[31m-[m
 Views/Admin/EditJob.cshtml            |  80 [32m++++++[m[31m------[m
 Views/Admin/Index.cshtml              |  28 [32m++[m[31m---[m
 Views/Admin/Jobs.cshtml               |  20 [32m+[m[31m--[m
 Views/Home/Index.cshtml               |  18 [32m+[m[31m--[m
 Views/Jobs/Apply.cshtml               |  58 [32m++++[m[31m-----[m
 Views/Jobs/Details.cshtml             |  46 [32m+++[m[31m----[m
 Views/Jobs/Index.cshtml               |  24 [32m++[m[31m--[m
 21 files changed, 559 insertions(+), 521 deletions(-)

[33mcommit 95463daef88ff0e951c04cb4938f27594b996eeb[m
Author: Tengano <blue.dragon.2k5@gmail.com>
Date:   Sun Oct 5 23:10:17 2025 +0700

    Chỉnh sửa lại 1 số cơ cấu

 Areas/Identity/Pages/Account/Login.cshtml        |  53 [31m----[m
 Areas/Identity/Pages/Account/Login.cshtml.cs     |  88 [31m------[m
 Areas/Identity/Pages/Account/Logout.cshtml       |  23 [31m--[m
 Areas/Identity/Pages/Account/Logout.cshtml.cs    |  32 [31m---[m
 Areas/Identity/Pages/Account/Register.cshtml     |  51 [31m----[m
 Areas/Identity/Pages/Account/Register.cshtml.cs  | 115 [31m--------[m
 Areas/Identity/Pages/Shared/_LoginPartial.cshtml |  34 [31m---[m
 Areas/Identity/Pages/_ViewImports.cshtml         |   4 [31m-[m
 Areas/Identity/Pages/_ViewStart.cshtml           |   3 [31m-[m
 CompleteDatabase.sql                             | 327 [32m+++++++++++++++++++++++[m
 Controllers/AccountController.cs                 |  95 [32m+++++++[m
 Controllers/AdminController.cs                   |  91 [32m++++[m[31m---[m
 Controllers/HomeController.cs                    |   6 [32m+[m[31m-[m
 Controllers/JobsController.cs                    |  83 [32m+++[m[31m---[m
 Data/ApplicationDbContext.cs                     |  67 [32m++++[m[31m-[m
 Models/Application.cs                            |  33 [32m++[m[31m-[m
 Models/Candidate.cs                              |  22 [32m+[m[31m-[m
 Models/Job.cs                                    |  16 [32m++[m
 Models/SeedData.cs                               |  37 [31m---[m
 Models/User.cs                                   |  33 [32m+++[m
 Program.cs                                       |  33 [32m+[m[31m--[m
 Services/A