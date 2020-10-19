import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TagProductSearchView } from '../../_model/print-inprocess-tag';
import { DatePipe } from '@angular/common';
import { PrintInprocessTagService } from '../../_service/print-inprocess-tag.service';

@Component({
  selector: 'app-inprocess-tag-product-search',
  templateUrl: './inprocess-tag-product-search.component.html',
  styleUrls: ['./inprocess-tag-product-search.component.scss']
})
export class InprocessTagProductSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: TagProductSearchView,
    private _fb: FormBuilder,
    private _printTagSvc: PrintInprocessTagService,
    private cdr: ChangeDetectorRef,
  ) { }

  public validationForm: FormGroup;
  user:any = {};
  public model : TagProductSearchView  = new TagProductSearchView();
  //datas: any;
  datas: any = {};
 

  async ngOnInit() {
    //this.buildForm();
    var datePipe = new DatePipe("en-US");
    //this.model.req_date  = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
    this.model.req_date = this.data.req_date; 
    this.model.pdjit_grp = this.data.pdjit_grp;
    //this.model.wc_code = this.data.wc_code;
    console.log(this.model);

    this.datas = await this._printTagSvc.getproduct(this.model);
    //this.datas = await this._jobInprocessSvc.getproduct(this.model);
    console.log(this.datas);
    //this.search();
        
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
