import { NgClass } from '@angular/common';
import { Component } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormGroup,FormControlName,ReactiveFormsModule,FormControl} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Filter } from '../../Interface/Filter';
import { AttendeeService } from '../../Services/AttendeeService';
import { Lookup } from '../../Services/lookup';
import { EventService } from '../../Services/event-service';
import { EventObj } from '../../Interface/EventObj';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-event',
  imports: [ReactiveFormsModule, NgClass, NgxPaginationModule,DatePipe],

  templateUrl: './event.html',
  styleUrl: './event.css',
})
export class EventComponent {
  constructor(private _allOrganizers:AttendeeService,private _places:Lookup,private _eventService:EventService){

  }
  ngOnInit(){
    this.click();//load all events at the beginning
    this.loadAllOrganizers();
    this.loadAllPlaces();


  }
  loadAllPlaces(){
    this.allLocations=[{Id:null,Name:'Choose Location'},];
    this._places.GetAllPlaces().subscribe({
      

      next:(res:any)=>{
        res.forEach((x:any)=>{
          let place:Filter={Id:x.id,Name:x.name}
          this.allLocations.push(place);
        });
      },
      error:(err:any)=>{
        console.log(err);
      }
    });
  }
  loadAllOrganizers(){
    this.allOrganizers=[
      {Id:null,Name:'Choose Organizer'},
    ];
    this._allOrganizers.getAllOrganizers().subscribe({
      next:(data:any)=>{
        data.forEach((x:any)=>{
          let organizer:Filter={Id:x.id,Name:x.name}
          this.allOrganizers.push(organizer);

        }
        
        );
      
      },
      error:(err:any)=>{
        console.log(err);
      }
    });



  }

  allOrganizers:Filter[]=[];
  allLocations:Filter[]=[];
  allEvents:EventObj[]=[];

  searchEventForm:FormGroup=new FormGroup({
    title:new FormControl<string | null>(null),
    eventDate:new FormControl<string | null>(null),
    organizerId:new FormControl<number | null>(null),
    createdAt:new FormControl<string | null>(null),
    LocationId:new FormControl<number | null>(null),
 
  });
   paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }
  changePage(pageNumber:number){
    this.paginationConfig.currentPage=pageNumber;
  }

  click(){
    this.allEvents=[];
    this._eventService.getAllEvents(this.searchEventForm.value).subscribe({

      next:(res:any)=>{
        res.forEach((x:any)=>{
          let eventObj:EventObj={
            Title:x.title,
            Description:x.description,
            LocationName:x.locationName,
            LocationId:x.locationId,
            EventDate:x.eventDate,
            CreatedAt:x.createdAt,
            Id:x.id,
           
            OrganizerName:x.organizerName, 
            maxParticipants:x.maxParticipants,
            OrganizerId:x.organizerId,
          }
          this.allEvents.push(eventObj);

        })

      },
      error:(err:any)=>{}

      
    });
    

  }

}
