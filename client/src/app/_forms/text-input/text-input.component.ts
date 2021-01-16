import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
// access the forms controls in our application
export class TextInputComponent implements ControlValueAccessor {
  @Input() label: string;
  @Input() type = 'text';

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
   }

  // these are the require methods that we need to have but we don't touch them
  writeValue(obj: any): void {
  }
  registerOnChange(obj: any): void {
  }
  registerOnTouched(obj: any): void {
  }
}
