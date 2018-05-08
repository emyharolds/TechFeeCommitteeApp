# TechFeeCommitteeApp
The UCM Harmon College of Business and Professional Studies needs automated system to evaluate and review requests made by departments on the use of its Students’ Technology Fees for the provision of relevant technology needs in the College and submit their recommendations to the Dean.
The project is aimed at automating the process. 

Technologies Implemented
	Client-side: HTML, CSS(Bootstrap), JavaScript, jQuery, ASP Razor View Pages
	Server-side: ASP.NET Web MVC && Entity Framework Code First Migrations
	Database Platform/Framework: SQL Server 2014
	Web Server Platform: Internet Information Services (IIS)
	Development Tools: Visual Studio 2015

Description of Users
a.	Committee Chairman: This user is the appointed Head of the Student Technology Fee Committee tasked with collecting, evaluating and recommending requests to the Dean for the application of the School’s Technology Fee paid by students.
b.	Committee Members: This user group consists of faculty appointed as representatives, each from every department in the Harmon College of Business and Professional Studies to serve on the Student Technology Fee Committee.
c.	Department Chairs: This user group consists of Heads of Departments in the Harmon College of Business and Professional Studies.
d.	Faculty: This user group consists of all faculty in the departments housed under the Harmon College of Business and Professional Studies.
e.	Dean: This user is the appointed Dean of the Harmon College of Business and Professional Studies.

Domain Analysis
a.	Business Objects:
•	Request Form: This form captures the contents of a request submitted by any user in the system. To fully describe a request, the user submits inputs for fields in the form such as Item Description, Item Usage, Justification of Request, and others.
•	Request Status: This object defines the stage of processing for each given request. This includes states such as Approved, Denied, Awaiting Departmental Chair Review, and so on.
•	Periods: This object is used to describe the different phases of the application lifecycle. It is sequential and consists of: Setup, Submission, Departmental Review, Committee Review, Committee Chair Comments and Dean’s Review. Roles, features, and access are configured for each period.
•	Votes: This object denotes a Committee Member’s approval or disapproval of a submitted request under review by the Committee. A committee member may choose a “Yes”, “No”, “Yes with Reservations” or “Need More Information”.
•	Faculty Access: This object describes the capability of the Faculty user group to access certain features during different periods in the application lifecycle.
•	Generic Password: This is a default password created by the Committee Chairman while setting up the accounts of the Committee Members. Members are required to change the default passwords if they are accessing the system a first time.

b.	Business Rules:
•	Only one user in the system can hold a Committee Chairman or Dean role. These roles cannot be assigned to several users.
•	Every Committee Member and Department Chair role is assigned to one user per department in the system.
•	A Committee Member may only submit one vote per request.
•	The Committee Chairman may create and manage all accounts of the Committee Members in the system. Other user accounts are uploaded into the system prior to Setup.
•	Only requests approved by each Department Chair may be forwarded to the Committee Members for review.
•	Since all users in the system also hold the Faculty role, they may submit one or more requests on the system.
•	All Faculty users may not edit their requests after the submission phase. They may, however, log in and view the status of their requests.
•	All requests reviewed by the Committee are forwarded to the Dean for final review, whether recommended by the Committee Chairman or not.

