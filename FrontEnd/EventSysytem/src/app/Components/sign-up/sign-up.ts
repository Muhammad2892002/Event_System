import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule, ɵInternalFormsSharedModule } from '@angular/forms';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-sign-up',
  imports: [RouterLink, ɵInternalFormsSharedModule,ReactiveFormsModule],
  templateUrl: './sign-up.html',
  styleUrl: './sign-up.css',
})
export class SignUp {




     AttendeeEditForm:FormGroup=new FormGroup({
    userName: new FormControl<string>("",Validators.required),
    password:new FormControl<string|null>(null),
    id:new FormControl<number|null>(null),
      firstName:new FormControl<string>("",Validators.required),
      lastName:new FormControl<string>("",Validators.required),
        email:new FormControl<string>("",Validators.required),
         userId:new FormControl<number|null>(null),
         gender:new FormControl<boolean>(false,Validators.required),
           roleId:new FormControl<number|null>(null),
            isActive:new FormControl<boolean>(false),

      
   

  });


  showForms(){
    console.log(this.AttendeeEditForm.value);
  }

}
