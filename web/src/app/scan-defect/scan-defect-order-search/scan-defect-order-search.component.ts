import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OrderReqSearchView } from '../../_model/job-operation-stock';
import { ScanDefectService } from '../../_service/scan-defect.service';

@Component({
  selector: 'app-scan-defect-order-search',
  templateUrl: './scan-defect-order-search.component.html',
  styleUrls: ['./scan-defect-order-search.component.scss']
})
export class ScanDefectOrderSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: OrderReqSearchView,
    private _fb: FormBuilder,
    private _defectSvc: ScanDefectService,
    private cdr: ChangeDetectorRef,
  ) { }

  public validationForm: FormGroup;
  user:any = {};
  public model : OrderReqSearchView  = new OrderReqSearchView();
  //datas: any;
  datas: any = {};
 

  async ngOnInit() {
    this.buildForm();
    
    
    this.model.wc_code = this.data.wc_code;
    
    
    
    console.log(this.model);
    //this.datas = await this._jobSvc.getOrderReqAll(this.model);
    console.log(this.datas);
   
        
  }

  private buildForm() {
    this.validationForm = this._fb.group({
    por_no: [null, []]
    });
  }

  async search() {
    this.model.por_no = this.model.por_no.toUpperCase();    
    this.datas = await this._defectSvc.getOrderReq(this.model);

  }

  add()
  { 
    let addList: any = this.datas.datas.filter(x => x.selected);
    //console.log(addList);

    this.dialogRef.close(addList);
  }

  close()
  {
    this.dialogRef.close([]);
  }

}
