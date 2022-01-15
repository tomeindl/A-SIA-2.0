import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenubarComponent } from './menubar.component';
import { MenubarMenuComponent } from './menubar-menu/menubar-menu.component';
import { MenubarEntryComponent } from './menubar-menu/menubar-entry/menubar-entry.component';
import { MenubarService } from './menubar.service';
import { SharedModule } from 'src/app/shared/shared.module';

/**
 * See MenubarComponent for usage
 */
@NgModule({
  declarations: [MenubarComponent, MenubarMenuComponent, MenubarEntryComponent],
  imports: [CommonModule, SharedModule],
  exports: [MenubarComponent],
  providers: [MenubarService]
})
export class MenubarModule {}
