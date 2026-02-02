import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';
import { AttendeeService } from '../../Services/AttendeeService';
import { Attendee } from '../../Interface/attendee';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-attendee',
  imports: [ReactiveFormsModule, NgClass, NgxPaginationModule],
  templateUrl: './attendee.html',
  styleUrl: './attendee.css',
})
export class AttendeeComponent {
  constructor(private _attendeeService: AttendeeService) { }

  searchForm: FormGroup = new FormGroup({
    FirstName: new FormControl<string | null>(null),
    LastName: new FormControl<string | null>(null),
    RoleId: new FormControl<number | null>(null),
    GenderId: new FormControl<boolean | null>(null),
    ISActivated: new FormControl<boolean | null>(null),


  });
  paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }
  ngOnInit(){
    this.click();//load all attendees at the beginning
  }

  changePage(pageNumber:number){
    this.paginationConfig.currentPage=pageNumber;
  }
  attendees: Attendee[] = [];
  click() {
    this.attendees = [];
    this._attendeeService.getByCriteria(this.searchForm.value).subscribe({
      next: (element: any) => {
        element.forEach((x:any)=>{
           let attendee: Attendee = {
              userName: x.userName,
              Id: x.id,
              
              firstName: x.firstName,
              lastName: x.lastName,
              email: x.email,
              userId: x.userId,
              genderId: x.gender,
              genderName: x.genderName,
              RoleId: x.role,
              RoleName: x.roleName,
              isActive: x.isActive,
              password: null,



            }
            this.attendees.push(attendee);
        })
        



       },

      error: err => {
        console.log(err.error?.message ?? err?.message ?? "Http Response Error");



      }



    });
    console.log(this.attendees);




  }

}
