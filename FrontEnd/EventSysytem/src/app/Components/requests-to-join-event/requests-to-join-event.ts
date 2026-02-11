import { Component } from '@angular/core';
import { FormControl, FormGroup,ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { Filter } from '../../Interface/Filter';
import { RequestToJoinEventService } from '../../Services/request-to-join-event-service';
import { EventRequest } from '../../Interface/event-request';
import { DatePipe } from '@angular/common';
import { AttendeeService } from '../../Services/AttendeeService';
import { EventService } from '../../Services/event-service';
import { AddAndUpdateRequest } from '../../Interface/AddRequestAndUpdate';
import { DeleteConfirmation } from '../delete-confirmation/delete-confirmation';

@Component({
  selector: 'app-requests-to-join-event',
  imports: [ReactiveFormsModule,NgxPaginationModule,DatePipe,DeleteConfirmation],
  templateUrl: './requests-to-join-event.html',
  styleUrl: './requests-to-join-event.css',
})
export class RequestsToJoinEvent {

 requestToJoinEvent:FormGroup=new FormGroup({
  eventId:new FormControl<number | null>(null),
  userId:new FormControl<number | null>(null),
  status:new FormControl<number | null>(null),
  requestedAt:new FormControl<string | null>(null),
 
  });
  constructor(private requestToJoinEventService:RequestToJoinEventService,
    private participantServ:AttendeeService,
   private eventServ: EventService) {}
      AllParticipants :Filter[]=[];
 
  allAttendee:Filter[]=[];
  allEvents:Filter[]=[];
  allrequests:EventRequest[]=[];
  ngOnInit(){
    this.getAllAttendee();
    this.getAllEvents();
    this.click();


  }

   paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }
  changePage(pageNumber:number){
    this.paginationConfig.currentPage=pageNumber;
  }

  click(){
    this.allrequests=[];
    this.requestToJoinEventService.GetAllRequest(this.requestToJoinEvent.value).subscribe({
      next:(res:any)=>{
        res.forEach((element:any)=>{
          let request:EventRequest={
            id:element.id,
            eventId:element.eventId,
            userId:element.userId,
            eventName:element.eventName,
            userName:element.name,
            email:element.email,
            status:element.status,
            statusName:element.statusName,
            requestedAt:element.requestedAt

          }
          this.allrequests.push(request);

        })

      },
      error:(err:any)=>{
        console.log(err);
      }
    });
    console.log(this.requestToJoinEvent.value);

  }

  getAllAttendee(){
    this.allAttendee=[{Id:null,Name:"Choose Attendee"}];
  this.requestToJoinEventService.GetAllAtendeeRequests().subscribe({
    next:(res:any)=>{
      res.forEach((element:any) => {
        let attendee:Filter={
          Id:element.participantUserId,
          Name:element.participantName
        }
        this.allAttendee.push(attendee);
      })
    },
    error:(err:any)=>{
      console.log(err);
    }



  });


  }


    getAllEvents(){
      this.allEvents=[{Id:null,Name:"Choose Event"}];
  this.requestToJoinEventService.GetAllEventsWithRequests().subscribe({
    next:(res:any)=>{
      res.forEach((element:any) => {
        let event:Filter={
          Id:element.eventId,
          Name:element.eventName
        }
        this.allEvents.push(event);
      })
    },
    error:(err:any)=>{
      console.log(err);
    }



  });


  }


  users = [
    { id: 1, name: 'Alice Johnson' },
    { id: 2, name: 'Bob Smith' },
    { id: 3, name: 'Charlie Brown' },
    { id: 4, name: 'Diana White' }
  ];

  events = [
    { id: 1, title: 'Angular Workshop' },
    { id: 2, title: 'React Conference' },
    { id: 3, title: 'Vue Meetup' },
    { id: 4, title: 'Fullstack Bootcamp' }
  ];

  // FormGroup with two dropdowns
  requestForm: FormGroup = new FormGroup({
    Id:new FormControl<number|null>(null),
    userId: new FormControl<number | null>(null, Validators.required),
    eventId: new FormControl<number | null>(null, Validators.required)
  });



 

    deleteConfirmation :boolean=false;
  title:string="Request delete confirmation ";
  content:string="Are you sure you want to delete this request";
  RequestToJoinEvent?:number|null=0;

  deleteJoinRequest(id?:number){
    this.RequestToJoinEvent=id;
    this.deleteConfirmation=true;
  }



  

  confirmJoinRequestDelete(confirm:boolean){
    if(confirm){
     this.requestToJoinEventService.DeleteRequestToJoinEvent(this.RequestToJoinEvent).subscribe({
        
      next:(res:any)=>{
        alert("Deleted successfully");
        this.deleteConfirmation=false;
        this.click();

      },
      error:err=>{
        alert(err.error.msg)
      }

     });
    }
    else{
      this.deleteConfirmation=false;
    }

  }







}
