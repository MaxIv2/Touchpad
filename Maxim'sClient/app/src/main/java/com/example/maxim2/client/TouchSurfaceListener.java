package com.example.maxim2.client;

import android.util.Log;
import android.view.MotionEvent;
import android.view.View;

/**
 * Created by Maxim2 on 4/5/2018.
 */

public class TouchSurfaceListener implements View.OnTouchListener {
    public TouchSurfaceListener(Client c) {
       this.c = c;
    }
    private Client c;
    private float previousX;
    private float previousY;
    private final byte[] bufferMove = {0,0,0};
    private final byte[] bufferScroll = {3,0};
    private final byte[] bufferPinch = {4,0};
    private static final int DX_INDEX = 1;
    private static final int DY_INDEX = 2;
    private static final int EXTRA_INDEX = 1;
    @Override
    public boolean onTouch(View view, MotionEvent motionEvent) {
        if(motionEvent.getAction() == MotionEvent.ACTION_DOWN) {
            this.previousX = motionEvent.getX();
            this.previousY = motionEvent.getY();
        }
        else if (motionEvent.getPointerCount() == 1)
            this.onOneFinger(motionEvent);
        else if(motionEvent.getPointerCount() == 2)
            this.onTwoFingers(motionEvent);
        return false;
    }

    private void onOneFinger(MotionEvent motionEvent) {
        if (motionEvent.getAction() == MotionEvent.ACTION_MOVE) {
            Log.d("move: ", "px: " + previousX + ", py: " + previousY + ", x: " + motionEvent.getX() + ", y:" + motionEvent.getY());
            int dx = (int) ((motionEvent.getX() - previousX));
            int dy = (int) ((motionEvent.getY() - previousY));
            Log.d("move:", "dx: " + (byte) dx + " dy :" + (byte) dy);
            bufferMove[DX_INDEX] = (byte) dx;
            bufferMove[DY_INDEX] = (byte) dy;
            this.previousX = motionEvent.getX();
            this.previousY = motionEvent.getY();
            c.addToQueue(bufferMove);
        }
    }
    private void onTwoFingers(MotionEvent motionEvent) {
        int action = motionEvent.getActionMasked();
        if(action == MotionEvent.ACTION_MOVE) {
            int pinch = isPinch(motionEvent);
            if(isPinch(motionEvent) != 0) {
                bufferPinch[EXTRA_INDEX] = (byte) pinch;
                c.addToQueue(bufferPinch);
            } else {
                int dy = (int) (motionEvent.getY() - previousY);
                bufferScroll[EXTRA_INDEX] = (byte) dy;
                c.addToQueue(bufferScroll);
                previousY = motionEvent.getY();
                Log.d("ispinch " , isPinch(motionEvent) + "");
            }
        }
    }

    private int isPinch(MotionEvent motionEvent) {
        if(motionEvent.getPointerCount() != 2)
            return 0;
        if(motionEvent.getHistorySize() < 1)
            return 0;
        //historical
        MotionEvent.PointerCoords h0 = new MotionEvent.PointerCoords();
        MotionEvent.PointerCoords h1 = new MotionEvent.PointerCoords();
        motionEvent.getHistoricalPointerCoords(0, 0, h0);
        motionEvent.getHistoricalPointerCoords(1, 0, h1);

        //actual
        MotionEvent.PointerCoords a0 = new MotionEvent.PointerCoords();
        MotionEvent.PointerCoords a1 = new MotionEvent.PointerCoords();
        motionEvent.getPointerCoords(0, a0);
        motionEvent.getPointerCoords(1, a1);

        MotionEvent.PointerCoords vector0to1= new MotionEvent.PointerCoords();
        vector0to1.x = a1.x - a0.x;
        vector0to1.y = a1.y - a0.y;

        MotionEvent.PointerCoords vector1Move = new MotionEvent.PointerCoords();
        vector1Move.x = h1.x - a1.x;
        vector1Move.y = h1.y - a1.y;

        MotionEvent.PointerCoords vector0Move = new MotionEvent.PointerCoords();
        vector0Move.x = a0.x - h0.x;
        vector0Move.y = a0.y - h0.y;
        double a = getAngle(vector0Move, vector0to1);
        if(a > Math.PI / 6 && a < Math.PI * 5 / 6)
            return 0;
        a = getAngle(vector1Move, vector0to1);
        if(a > Math.PI / 6 && a < Math.PI * 5 / 6)
            return 0;
        double res = Math.sqrt(Math.pow(vector0Move.x+ vector1Move.x, 2) + Math.pow(vector0Move.y+ vector1Move.y, 2));
        if (a <  Math.PI / 6)
            res = -res;
        return (int)res / 2;
    }

    private double getAngle(MotionEvent.PointerCoords vector1, MotionEvent.PointerCoords vector2) {
        float mone = vector1.x * vector2.x + vector1.y * vector2.y;
        double machane = Math.sqrt(Math.pow(vector1.x,2) + Math.pow(vector1.y,2)) * Math.sqrt(Math.pow(vector2.x,2) + Math.pow(vector2.y,2));
        return Math.acos(mone/machane);
    }

}
