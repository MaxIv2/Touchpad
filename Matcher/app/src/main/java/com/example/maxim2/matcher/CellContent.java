package com.example.maxim2.matcher;

import java.util.Random;

/**
 * Created by Maxim2 on 4/27/2018.
 */

public class CellContent {
    public enum ContentType {
        EMPTY,
        RED,
        BLUE,
        GREEN,
        YELLOW,
        PURPLE,
        ORANGE;
        public static ContentType getRandom() {
            Random r = new Random(values().length);
            return values()[r.nextInt()];
        }
    };
    private ContentType type;

    public CellContent(ContentType type) {
        this.type = type;
    }

    public ContentType getType() {
        return type;
    }
}
