import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NetworkPanelComponent } from './network-panel/network-panel.component';
import { DataPanelComponent } from './data-panel.component';
import { StructurePanelComponent } from './structure-panel/structure-panel.component';
import { InspectorPanelComponent } from './inspector-panel/inspector-panel.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [NetworkPanelComponent, DataPanelComponent, StructurePanelComponent, InspectorPanelComponent],
  imports: [CommonModule, SharedModule],
  exports: [DataPanelComponent]
})
export class DataPanelModule {}
