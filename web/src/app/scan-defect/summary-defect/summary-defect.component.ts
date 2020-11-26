import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DefectProductSubSearchView } from '../../_model/scan-defect';
import { ScanDefectService } from '../../_service/scan-defect.service';

@Component({
  selector: 'app-summary-defect',
  templateUrl: './summary-defect.component.html',
  styleUrls: ['./summary-defect.component.scss']
})
export class SummaryDefectComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: DefectProductSubSearchView,
    private _fb: FormBuilder,
    private _defectSvc: ScanDefectService,
    private cdr: ChangeDetectorRef,
  ) { }

  public validationForm: FormGroup;
  user:any = {};
  public model : DefectProductSubSearchView  = new DefectProductSubSearchView();
  //datas: any;
  datas: any = {};

  async ngOnInit() {
    var datePipe = new DatePipe("en-US");
    this.model.entity = this.data.entity;
    this.model.req_date  = this.data.req_date; 
    this.model.wc_code = this.data.wc_code;
    this.model.por_no = this.data.por_no;
    this.model.ref_no = this.data.ref_no;
    
    console.log(this.model);
    this.datas = await this._defectSvc.getSummaryDefect(this.model);
    console.log(this.datas);
  }

  close()
  {
    this.dialogRef.close([]);
  }

}
