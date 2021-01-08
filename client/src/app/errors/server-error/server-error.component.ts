import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {

  error: any;

  constructor(private router: Router) {
    // we can't access router in onInit method we do access through the constructor only
    const navigation = this.router.getCurrentNavigation();
    // check the navigation for safety since we don't know if we have all the information like if the user refresh the page
    // so we use ? as optional chaining
    this.error = navigation?.extras?.state?.error;
    
   }

  ngOnInit(): void {
  }

}
