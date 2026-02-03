import { Component } from '@angular/core';
import { FormControl, FormGroup,ReactiveFormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { Filter } from '../../Interface/Filter';
import { RequestToJoinEventService } from '../../Services/request-to-join-event-service';
import { EventRequest } from '../../Interface/event-request';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-requests-to-join-event',
  imports: [ReactiveFormsModule,NgxPaginationModule,DatePipe],
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
  constructor(private requestToJoinEventService:RequestToJoinEventService) {}
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

}
