import { Component } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormGroup, FormControl,ReactiveFormsModule, Validators } from '@angular/forms';
import { Filter } from '../../Interface/Filter';
import { EventParticipantsService } from '../../Services/EventParticipantsService';
import { EventParticipant } from '../../Interface/event-participant';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-event-participants',
  imports: [NgxPaginationModule, ReactiveFormsModule,DatePipe],
  templateUrl: './event-participants.html',
  styleUrl: './event-participants.css',
})
export class EventParticipants {
  changePage(pageNumber: number) {
    console.log('Changing page to:', pageNumber);
  }
  constructor(private eventParticipantsService: EventParticipantsService) {}
  searchParticipants:FormGroup=new FormGroup({
    participantId:new FormControl<number | null>(null),
    eventId:new FormControl<number | null>(null),
  });
  participant:Filter[]=[];
  events:Filter[]=[];
  allEventParticipants:EventParticipant[]=[];
  ngOnInit(){
    this.loadParticipantsInEvent();
    this.loadEventsForParticipant();
    this.click();
  }
   paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }

  click(){
    this.allEventParticipants=[];
    this.eventParticipantsService.GetAllParticipants(this.searchParticipants.value).subscribe({
      next:(res:any)=>{
        res.forEach((x:any)=>{
          let participant:EventParticipant={
            Id:x.id,
            organizerId:x.organizerId,
            locationId:x.locationId,
            AttendeeId:x.attendeeId,
             EventId:x.eventId,
            AttendeeName:x.attendeeName,
           email:x.email,
            EventName:x.eventName,
            
            locationName:x.location,
            HeldAt:x.heldAt,
            organizerName:x.organizerName,
            
            
            
          };
          this.allEventParticipants.push(participant);
        })
        console.log(this.allEventParticipants);
      },
      error:(err:any)=>{console.log(err);}
    });

    console.log(this.searchParticipants.value);
  }
  loadParticipantsInEvent(){
    this.participant = [{Id:null,Name:'Choose Participant'},];
    this.eventParticipantsService.getAllParticipantsInEvent().subscribe({
      next:(res:any)=>{
        res.forEach((x:any)=>{
          let participant:Filter={Id:x.participantUserId,Name:x.participantName}
          this.participant.push(participant);
        })
       
      },
      error:(err:any)=>{
        console.log(err);
      }
    });
 
  }
loadEventsForParticipant(){
  this.events=[{Id:null,Name:'Choose Event'},];

   this.eventParticipantsService.getAllEventsInParticipant().subscribe({
      next:(res:any)=>{
        res.forEach((x:any)=>{
          let event:Filter={Id:x.eventId,Name:x.eventName}
          this.events.push(event);
        })
        
      },
      error:(err:any)=>{
        console.log(err);
      }
    });

}
  participants = [
    { id: 1, name: 'Alice Johnson' },
    { id: 2, name: 'Bob Smith' },
    { id: 3, name: 'Charlie Brown' },
    { id: 4, name: 'Diana White' }
  ];

  eventsName = [
    { id: 1, title: 'Angular Workshop' },
    { id: 2, title: 'React Conference' },
    { id: 3, title: 'Vue Meetup' },
    { id: 4, title: 'Fullstack Bootcamp' }
  ];

  // FormGroup with two dropdowns
  participantEventForm: FormGroup = new FormGroup({
    participantId: new FormControl<number | null>(null, Validators.required),
    eventId: new FormControl<number | null>(null, Validators.required)
  });

    saveParticipantEvent() {
    if (this.participantEventForm.valid) {
      console.log('Selected:', this.participantEventForm.value);
      // Call your API or service here
    } else {
      console.log('Form is invalid');
    }
  }


}

