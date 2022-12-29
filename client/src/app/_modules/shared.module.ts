
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';




@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ToastrModule.forRoot({ positionClass: 'toast-bottom-right' }),
    BsDatepickerModule.forRoot(),
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule,

  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule,
    BsDatepickerModule
  ]
})
export class SharedModule { }
