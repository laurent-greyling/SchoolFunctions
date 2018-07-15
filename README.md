# SchoolFunctions

This is a small project to play with Azure Functions. The purpose of this Azure Function App is to upload courses, then allow a student to signup to the courses uploaded.

## Uploading Information
Uploading information such as course or student details are done by sending the below information to a queue. 
This queue would have been either created manually, ARM template or with your backend APP. For this excersise it was created manually, but preferably this should be done programatically. 

### Courses
When uploading courses you need the following JSON structure:

```
{  
   'MessageType':'UploadCourse',
   'Courses':[  
      {  
         'Course':'A',
         'Lecturer':'A',
         'MaxQuantity':30
      },
      {  
         'Course':'B',
         'Lecturer':'B',
         'MaxQuantity':30
      }
   ]
}

```

- __MessageType__ : This will indicate to the function what process it should follow. In this case uploading courses to the backend
- __Courses__ : A list of courses you want to upload for students to participate in

### Signup of Student
When a student signs up you need the following JSON Structure:

```
{  
   'MessageType':'SignUp',
   'Details':{  
      'Name':'Name',
      'Surname':'Surname',
      'Email':'email@email.com',
      'Age':35,
      'Course':'Awesome Course C'
   }
}
```

- __MessageType__ : This will indicate to the function what process it should follow. In This case it should signup a student
- __Details__ : Student details for signup

### Upload Info and retrieve Info Script
In general you will send the above information via your own API service to the queue. In this case you can send the information to the queue via the `\Scripts\QueueMessage.psm1`.

To make this script work:

- Install-Module AzureRmStorageQueue
- Install-Module AzureRmStorageTable
- Login-AzureRMAccount -TenantId "your tenant id"
- Import-Module QueueMessage.psm1 -Force
- Send-QueueMessage -ResourceGroupName "ResourceGroupName" -UserDetails "your json string"
  - This will upload the info for course or student
- Retrieve-SignupDetails -Name "Student Name" -Surname "Student Surname" -Course "Course Name"
  - This will check if student sighup was success or not and give reason for it

## The Function
The function called `ManageFunction.cs` will be triggered once a message is on the queue. If the messagetype matches anything the fucntion knows, it will redirect it to the correct message handler to process the information.

### UploadCourseMessageHandler
This will run if the message type is `UploadCourse`. This will then proceed to upload the course(s) and write temetry data to Application Insights

### SignUpMessageHandler
This will run if the message type is `SignUp`. This will then start the process of uploading student information, send email to student if correct email is given and write telemetry information about the student.

## Telemetry Information via Application Insights
Some telemetry information is written to Application Insight

## Exceptions
Exceptions are logged in AI, this can be queried via the portal

## Upload course info
Basic info is logged, although I find this info a bit useless

## Signup Info
Signup info that is logged are as followed

- Name of student
- Surname of student
- Email
- Age
- Course

__Note__ not GDPR compliant, for this reason I suggest only logging Age and Course as we want to get the Max, Min and Average age for a specific course.

This details will the be used to query

- Course
- Max Age for this course
- Min Age for this course
- AVG Age for this course

### Query Application Insights
To get the needed iformation from application insights you can query for it in your application insights portal.

The query to get the information we need as discussed above is as follows

```
customEvents
| where name == "SignUp" and customDimensions.Course == 'Awesome Course E'
| summarize Course = any(tostring(customDimensions.Course)), ['Maximum Age'] = max(toint(customDimensions.Age)), ['Minimum Age'] = min(toint(customDimensions.Age)), ['Average Age'] = avg(toint(customDimensions.Age)) 
```

You can then pin this to your Azure Portal DashBoard and have the following:

![image](https://user-images.githubusercontent.com/17876815/42734142-53c2d8b4-883f-11e8-92da-da4ae8b95c46.png)


##Some (Obvious) Things to improve
- Currently using storage queue, would like to use Service Bus queue.
- Some more backend validation can be done, like checking student details not null or empty
- Currently write back the student feedback into Signup table. This can be different table or even better back onto a queue for processing
