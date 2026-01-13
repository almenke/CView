# CView - Stakeholder View
CView is an application that builds a single horizontally scrollable Stakeholder View of a project.  It is built to create project timeline visualizations for the project stakeholders.  Version 1 has minimal functionality but should provide a way to uploade a spreadsheet of tasks and put them into webpage visual that is easy to read and visualize by non-technical people.

The webpage visual will have 6 columns on the lowest level that represent 6 months.  It will have 2 sprints that start and end in each of the month columns.  Even though the end dates of the second sprint of the month does not alwasy end within the month, that's okay because the visualization still works.  The sprints are two weeks each.  Inside the sprints there will tasks.  The earlist starat dates will be at the top and then the rest will go down accordingly.  The visual looks like a gantt chart with the other month and sprint layers under.  Only the subtasks of the project uploaded will be shown on the page.  This is determined by the outline level in the spreadsheet.  If there is a decimal and a number then that task will be displayed.  For the first cut, we are not displaying the owners of the task.  That will be in version 2. 

For our first upload, the start and end dates on the spreadsheet will load into PlannedStartDate

## Functionality
1. Manage Projects
2. Manage Owners (Task Owner or )
3. Manage Project Sprints
4. Upload Tasks
5. Display Project Timeline

## Entities
* BaseEntity
* Project
* Sprint
* Owner
* Task

### BaseEntity
* CreatedAt
* UpdatedAt

### Project : BaseEntity
* Id
* Name
* OwnerId
* StartsAt
* EndsAt

### Sprint : BaseEntity
* Id
* ProjectId
* Name
* StartsAt
* EndsAt

### Owner : BaseEntity
* Id
* ProjectId
* Name
* Title

### Task : BaseEntity
* Id
* ProjectId
* Name
* OwnerId
* PlannedStartsAt
* PlannedEndsAt
* ActualStartsAt
* ActualStartsAt
* Status: StatusEnum

### StatusEnum
* Not Set = 0
* Backlog = 1
* Active = 2
* Complete = 3


     