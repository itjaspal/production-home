import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA, PageEvent } from '@angular/material';
import { Router } from '@angular/router';
import { SpecDrawingParamView, SpecDrawingView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOrderSummaryService } from '../../_service/job-order-summary.service';

@Component({
  selector: 'app-view-spec',
  templateUrl: './view-spec.component.html',
  styleUrls: ['./view-spec.component.scss']
})
export class ViewSpecComponent implements OnInit {

  public specDrawingData: SpecDrawingView; //= new SpecDrawingView();
  
  public user: any;
  public imageSource: any;
  public datePipe = new DatePipe('en-US');
  public vSum: number;
  public validationForm: FormGroup;

         
      
  //public element: HTMLElement;

  public scanPcsModel: any = { 
    scan_barcode: "",
}

  constructor(
    private _jobOrderSummarySvc: JobOrderSummaryService,
    private _authSvc: AuthenticationService,
    private _formBuilder: FormBuilder,
    private _router: Router,
    public dialogRef: MatDialogRef<any>,
    private _dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public paramData: SpecDrawingParamView

  ) { }

  ngOnInit() {
      this.buildForm(); 
      this.user = this._authSvc.getLoginUser();

      console.log("paramData.bar_code: " + this.paramData.bar_code);

      this.scanPcsModel.scan_barcode = this.paramData.bar_code;
      this.searchViewSpecDrawing(this.scanPcsModel.scan_barcode );
  }

  close() { 
    //window.history.back();
    this.dialogRef.close();
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      scan_barcode: ['', [Validators.required]]
    });
  }

 
  async searchViewSpecDrawing(p_barCode: string) { 

    if (p_barCode != "") { 
         console.log("searchSpecDrawingByPcs : " + p_barCode);
      
         this.specDrawingData = new SpecDrawingView();
 
         this.specDrawingData =  await this._jobOrderSummarySvc.getSpecDrawing(p_barCode);
         console.log(this.specDrawingData);
 
     } 
    sessionStorage.setItem('spect-drawing-barcode', p_barCode);
     
   }


  async searchViewSpec(event: PageEvent = null) { 

   
     
        this.specDrawingData = new SpecDrawingView();

        this.specDrawingData =  await this._jobOrderSummarySvc.getSpecDrawing(this.paramData.bar_code);
        console.log(this.specDrawingData);

    } 
  
    
  

}
