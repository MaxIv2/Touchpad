package com.example.nagli.touchpadclient;

import android.content.Intent;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;

public class StartActivity extends AppCompatActivity {
    Button btnScan;
    LinearLayout linearLayout;
    String contents;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_start);
        btnScan = (Button)findViewById(R.id.btnswitch);
        btnScan.setOnClickListener(btnScanOnClickListener);
    }
    View.OnClickListener btnScanOnClickListener = new View.OnClickListener()
    {
        @Override
        public void onClick(View view) {
            if(btnScan == view) {
                switchActivity(MainActivity.class);
            }
        }
    };
    public void switchActivity(Class C)
    {
        Intent intent = new Intent(this, C);
        intent.putExtra("contents", contents);
        startActivity(intent);
    }
}
