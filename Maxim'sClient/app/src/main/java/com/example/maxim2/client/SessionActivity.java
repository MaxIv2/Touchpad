package com.example.maxim2.client;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SessionActivity extends AppCompatActivity implements Client.SessionEndEventHandler {

    private Button leftButton;
    private Button rightButton;
    private Client client;
    private View surface;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_session);
        this.leftButton = findViewById(R.id.leftButton);
        this.rightButton = findViewById(R.id.rightButton);
        this.surface = findViewById(R.id.touchSurface);

        String qrData = getIntent().getExtras().getString(getString(R.string.EXTRA_QR_DATA));
        if(IsTCPEndpoint(qrData)) {
            try {
                this.client = new TcpClient(qrData.split(":")[0], Integer.parseInt(qrData.split(":")[1]),this) ;
            } catch (Exception e) {
                this.finish();
                return;
            }
        } else if(IsMACADress(qrData)){
            try {
                this.client = new BluetoothClient(qrData, this);
            } catch (Exception e) {
                this.finish();
                return;
            }
        }
        leftButton.setOnTouchListener(left);
        rightButton.setOnTouchListener(right);
        this.surface.setOnTouchListener(new TouchSurfaceListener(this.client));
    }

    private final View.OnTouchListener left = new View.OnTouchListener() {
        final byte[] leftDown = {1,0};
        final byte[] leftUp = {1,1};
        @Override
        public boolean onTouch(View view, MotionEvent motionEvent) {
            if(motionEvent.getAction() == MotionEvent.ACTION_DOWN)
                client.addToQueue(this.leftDown);
            if(motionEvent.getAction() == MotionEvent.ACTION_UP)
                client.addToQueue(this.leftUp);
            return false;
        }
    };
    private final View.OnTouchListener right = new View.OnTouchListener() {
        final byte[] rightDown = {2,0};
        final byte[] rightUp = {2,1};
        @Override
        public boolean onTouch(View view, MotionEvent motionEvent) {
            if(motionEvent.getAction() == MotionEvent.ACTION_DOWN)
                client.addToQueue(this.rightDown);
            if(motionEvent.getAction() == MotionEvent.ACTION_UP)
                client.addToQueue(this.rightUp);
            return false;
        }
    };


    final static private Pattern macPattern = Pattern.compile("(([0-9A-Fa-f]){2}:){5}([0-9A-Fa-f]){2}");
    private static boolean IsMACADress(String address) {
        Matcher matcher = macPattern.matcher(address);
        return matcher.matches();
    }
    private static final String zeroThrough255 = "[0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]";
    final static private Pattern ipPortPattern = Pattern.compile("((" +zeroThrough255 + ").){3}(" + zeroThrough255 +")");
    private static boolean IsTCPEndpoint(String endpoint){
        String[] parts = endpoint.split(":");
        if(parts.length!=2)
            return false;
        try {
            int p = Integer.parseInt(parts[1]);
            if(!(0<=p && p <=65535))
                return false;
        } catch (Exception e) {
            return false;
        }
        Matcher matcher = ipPortPattern.matcher(parts[0]);
        return matcher.matches();
    }

    @Override
    protected void onDestroy() {
        if(client != null)
            client.endSession();
        super.onDestroy();
    }

    @Override
    public void HandleSessionEnd() {
        this.finish();
    }
}