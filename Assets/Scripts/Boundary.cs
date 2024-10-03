/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: Boundary data type for restrictions
*/

public struct Boundary {
    public float Up, Down, Left, Right;
    public Boundary(float up, float down, float left, float right) {
        Up = up;
        Down = down;
        Left = left;
        Right = right;
    }
}
