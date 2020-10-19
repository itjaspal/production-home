import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, PageEvent } from '@angular/material';
import { Router } from '@angular/router';
import { SpecDrawingView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOrderSummaryService } from '../../_service/job-order-summary.service';

@Component({
  selector: 'app-spec-drawing',
  templateUrl: './spec-drawing.component.html',
  styleUrls: ['./spec-drawing.component.scss']
})
export class SpecDrawingComponent implements OnInit {



  public specDrawingData: SpecDrawingView; //= new SpecDrawingView();
  
  public user: any;
  public imageSource: any;
  public datePipe = new DatePipe('en-US');
  public vSum: number;
  public validationForm: FormGroup;
  


  @ViewChild('scan_barcode') scan_barcode: ElementRef;
        
      
  //public element: HTMLElement;



  public scanPcsModel: any = { 
      scan_barcode: "",
  }


  constructor(
    private _jobOrderSummarySvc: JobOrderSummaryService,
    private _authSvc: AuthenticationService,
    private _formBuilder: FormBuilder,
    private _router: Router,

  ) { }

  ngOnInit() {
      this.buildForm(); 
      this.user = this._authSvc.getLoginUser();
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      scan_barcode: ['', [Validators.required]]
    });
  }

 


  async searchViewSpec(event: PageEvent = null) { 

   if (this.scan_barcode.nativeElement.value != "") { 
        console.log("searchSpecDrawingByPcs : " + this.scan_barcode.nativeElement.value);
     
        this.specDrawingData = new SpecDrawingView();

        this.specDrawingData =  await this._jobOrderSummarySvc.getSpecDrawing(this.scan_barcode.nativeElement.value);
        console.log(this.specDrawingData);

    } 
   sessionStorage.setItem('spect-drawing-barcode', this.scan_barcode.nativeElement.value);
    
  }

}
