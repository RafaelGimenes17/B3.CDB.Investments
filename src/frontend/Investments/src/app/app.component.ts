import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CDBCalculatorComponent } from './cdb-calculator/cdb-calculator.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, CDBCalculatorComponent],
  template: '<app-cdb-calculator></app-cdb-calculator>',
  styles: []
})
export class AppComponent {
  title = 'Investments';
}
