import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,RouterLink,RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'EventSysytem';
constructor(public router :Router){}
    showNavbar():boolean{
   
  return this.router.url !== '/login' && this.router.url !== '/sign-up';


  


  }

}
//hello world
//ryhj56uj57i