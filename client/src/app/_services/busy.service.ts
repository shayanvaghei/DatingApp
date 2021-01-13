import { Injectable } from '@angular/core';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;
  constructor(private ngxService: NgxUiLoaderService) { }

  busy() {
    this.busyRequestCount++;
    //this.ngxService.sh
    this.ngxService.start();
  }

  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.ngxService.stop();
    }
  }
}
