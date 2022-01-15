import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from './button/button.component';
import { ButtonSmallComponent } from './button-small/button-small.component';
import { SliderComponent } from './slider/slider.component';
import { DropDownComponent } from './drop-down/drop-down.component';
import { InputSmallComponent } from './input-small/input-small.component';
import { InputMediumComponent } from './input-medium/input-medium.component';
import { InputLargeComponent } from './input-large/input-large.component';
import { ModalComponent } from './modal/modal.component';
import { ColorInputComponent } from './color-input/color-input.component';
import { ModalService } from './modal/modal.service';
import { FormsModule } from '@angular/forms';
import { InputMediumDirective } from './input-medium/input-medium.directive';

@NgModule({
  declarations: [
    ButtonComponent,
    ButtonSmallComponent,
    SliderComponent,
    DropDownComponent,
    InputSmallComponent,
    InputMediumComponent,
    InputLargeComponent,
    ModalComponent,
    ColorInputComponent,
    InputMediumDirective
  ],
  imports: [CommonModule, FormsModule],
  exports: [
    ButtonComponent,
    ButtonSmallComponent,
    SliderComponent,
    DropDownComponent,
    InputSmallComponent,
    InputMediumComponent,
    InputLargeComponent,
    ModalComponent,
    ColorInputComponent,
    InputMediumDirective
  ],
  providers: [ModalService]
})
export class SharedModule {}
