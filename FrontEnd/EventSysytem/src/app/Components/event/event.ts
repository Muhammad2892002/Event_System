import { NgClass } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormGroup, FormControlName, ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Filter } from '../../Interface/Filter';
import { AttendeeService } from '../../Services/AttendeeService';
import { Lookup } from '../../Services/lookup';
import { EventService } from '../../Services/event-service';
import { EventObj } from '../../Interface/EventObj';
import { DatePipe } from '@angular/common';
import { DeleteConfirmation } from '../delete-confirmation/delete-confirmation';


@Component({
  selector: 'app-event',
  imports: [ReactiveFormsModule, NgClass, NgxPaginationModule, DatePipe, DeleteConfirmation],

  templateUrl: './event.html',
  styleUrl: './event.css',
})
export class EventComponent {
  constructor(private _allOrganizers: AttendeeService, private _places: Lookup, private _eventService: EventService) {

  }
  @ViewChild('DeleteDialog') closeDialog!: ElementRef;
  ngOnInit() {
    this.click();//load all events at the beginning
    this.loadAllOrganizers();
    this.loadAllPlaces();


  }
  loadAllPlaces() {
    this.allLocations = [{ Id: null, Name: 'Choose Location' },];
    this._places.GetAllPlaces().subscribe({


      next: (res: any) => {
        res.forEach((x: any) => {
          let place: Filter = { Id: x.id, Name: x.name }
          this.allLocations.push(place);
        });
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }
  loadAllOrganizers() {
    this.allOrganizers = [
      { Id: null, Name: 'Choose Organizer' },
    ];
    this._allOrganizers.getAllOrganizers().subscribe({
      next: (data: any) => {
        data.forEach((x: any) => {
          let organizer: Filter = { Id: x.id, Name: x.name }
          this.allOrganizers.push(organizer);

        }

        );

      },
      error: (err: any) => {
        console.log(err);
      }
    });



  }

  allOrganizers: Filter[] = [];
  allLocations: Filter[] = [];
  allEvents: EventObj[] = [];

  searchEventForm: FormGroup = new FormGroup({
    Id: new FormControl<number | null>(null),
    title: new FormControl<string | null>(null),
    eventDate: new FormControl<string | null>(null),
    organizerId: new FormControl<number | null>(null),
    createdAt: new FormControl<string | null>(null),
    LocationId: new FormControl<number | null>(null),

  });
  paginationConfig = {
    itemsPerPage: 5,
    currentPage: 1,
  }
  changePage(pageNumber: number) {
    this.paginationConfig.currentPage = pageNumber;
  }

  click() {
    this.allEvents = [];
    this._eventService.getAllEvents(this.searchEventForm.value).subscribe({

      next: (res: any) => {
        res.forEach((x: any) => {
          let eventObj: EventObj = {
            Title: x.title,
            Description: x.description,
            LocationName: x.locationName,
            Location: x.locationId,
            EventDate: x.eventDate,
            CreatedAt: x.createdAt,
            Id: x.id,

            OrganizerName: x.organizerName,
            maxParticipants: x.maxParticipants,
            OrganizerId: x.organizerId,
          }
          this.allEvents.push(eventObj);

        })

      },
      error: (err: any) => { }


    });


  }

  locations = [
    { id: 1, name: 'Conference Hall A' },
    { id: 2, name: 'Conference Hall B' },
    { id: 3, name: 'Auditorium' },
    { id: 4, name: 'Outdoor Garden' }
  ];

  organizers = [
    { id: 1, name: 'John Doe' },
    { id: 2, name: 'Jane Smith' },
    { id: 3, name: 'Marketing Team' },
    { id: 4, name: 'Event Management Co.' }
  ];

  // Typed FormGroup
  eventForm: FormGroup = new FormGroup({
    id: new FormControl<number | null>(null),
    title: new FormControl<string | null>(null, Validators.required),
    eventDate: new FormControl<string | null>(null, Validators.required), // date as string
    locationId: new FormControl<number | null>(null, Validators.required),
    organizerId: new FormControl<number | null>(null),
    maxParticipants: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    description: new FormControl<string | null>(null)
  });

  saveEvent() {

    let eventObj: EventObj = {
      Id: this.eventForm.value.id ?? 0,
      Title: this.eventForm.value.title,
      Description: this.eventForm.value.description ?? null,
      Location: this.eventForm.value.locationId,
      EventDate: this.eventForm.value.eventDate,
      maxParticipants: this.eventForm.value.maxParticipants,
      OrganizerId: this.eventForm.value.organizerId,



    }
    if (this.eventForm.value.id == null) {
      this._eventService.AddEvent(eventObj).subscribe({
        next: (res: any) => {
          alert("Added successfully");
          this.click();
          this.closeDialog.nativeElement.click();
        },
        error: err => {
          console.log(err.error);

          alert(
            (err.error.errorMassageMaxParticipantsErrorMsg ?? "") + "\n" +
            (err.error.errorDateErrorMsg ?? "") + "\n" +
            (err.error.organizerError ?? "")
          );





        }





      });

    }
    else {
      this._eventService.UpdateEvent(eventObj).subscribe({

        next: (res: any) => {
          alert("Updated successfully");
          this.click();
          this.closeDialog.nativeElement.click();
        },
        error: err => {
          console.log(err.error);
          alert(
            (err.error.errorMassageMaxParticipantsErrorMsg ?? "") + "\n" +
            (err.error.errorDateErrorMsg ?? "") + "\n" +
            (err.error.organizerError ?? "")
          );
        }
      })




    }





  }
  clearForm() {
    this.eventForm.reset();
  }
  loadEvent(id?: number|null) {
    this.clearForm();
    this._eventService.getEventById(id??0).subscribe({
      next: (res: any) => {
        this.eventForm.patchValue({
          id: res.id,
          title: res.title,
          eventDate: res.eventDate,
          locationId: res.location,
          organizerId: res.organizerId,
          maxParticipants: res.maxParticipants,
          description: res.description
        });
      },
      error: err => { }
    })
  }
  deleteConfirmation: boolean = false;
  title = "Event delete confirmation";
  content = "Are you sure you wants to delete this event?";
  eventId?: number | null = 0;

  confirmAttendeeDelete(event: boolean) {

    if (event) {
      
      this._eventService.DeleteEvent(this.eventId).subscribe({

        next: (res: any) => {
          alert("Deleted successfully");
          this.click();
          this.deleteConfirmation = false;

        },
        error: err => {
           this.deleteConfirmation = false;
          alert("cant be deleted");

        }
      });
    }
    else {
      this.deleteConfirmation = false;


    }
  }

  showDeleteConfirmation(Id?: number) {
    this.deleteConfirmation = true;
    this.eventId = Id;



  }

  RoleCheck(){

    return localStorage.getItem("Role")?.toUpperCase()=="admin".toUpperCase();
  }

}
