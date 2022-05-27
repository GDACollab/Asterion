using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handles the text management of 1 section of the credits
public class CreditsSection : Object
{
    /*
     * The text boxes to be used
     * 
     * 0 = header: the name of the section
     * 1 = roles: what the people being credited did
     * 2 = people: the people being credited
     */

    private TextMeshProUGUI[] textBoxes;

    /**
     * Constructor
     * 
     * @Param textBoxes: an array of the 3 text objects used: header, roles, people
     * @Param postion: the initial position the text boxes are to be set to
     */
    public CreditsSection(TextMeshProUGUI[] textBoxes)
    {
        this.textBoxes = textBoxes;
    }

    /**
     * Adds a name-role combonation to the credits
     * 
     * @Param name: the person being credited
     * @Param role: what the person did
     * 
     * Precondition: the people and roles text boxes must have the same number of rows
     * Precondition: the Content Size Fitter for both text boxes must be enabled
     */
    public void addCredit(string name, string role)
    {
        textBoxes[1].text += role + '\n';
        textBoxes[2].text += name + '\n';
    }

    /**
     * Sets the position of the 3 text boxes
     * Also forces their transforms to update for more accurate positioning
     * 
     * @Param position: the top center of the header text box
     * @Param margins: the distance between the text boxes, both vertical and horizontal
     */
    public void setPosition(Vector2 position, float margins)
    {
        setTextPosition(textBoxes[0], new Vector2(position.x, position.y));
        setTextPosition(textBoxes[1], new Vector2(position.x + (margins/2), position.y - textBoxes[0].rectTransform.rect.height - margins));
        setTextPosition(textBoxes[2], new Vector2(position.x - (margins/2), position.y - textBoxes[0].rectTransform.rect.height - margins));
    }

    /**
     * Sets the position of the provided text box
     * 
     * @Param text: the text box in question
     * @Param position: the position the text box will be set to
     */
    private void setTextPosition(TextMeshProUGUI text, Vector2 position)
    {
        text.transform.position = new Vector2(position.x, position.y);
    }

    /**
     * Translates the 3 text boxes up the provided distance
     * 
     * @Param distance: the distance the text box translated (positive for up, negative for down)
     */
    public void translateVertical(float distance)
    {
        foreach(TextMeshProUGUI textBox in textBoxes)
        {
            Vector2 position = textBox.transform.position;
            setTextPosition(textBox, new Vector2(position.x, position.y + distance));
        }
    }

    // Returns the height of the 3 text boxes by subtracting the y position of the bottom from the y position of the top
    public float getHeight()
    {
        return textBoxes[0].transform.position.y - getBottom();
    }

    // Returns the y-value for the bottom of the lowest text box
    // Note: people and roles should be the lowest and identical, so this only gets the bottom of roles
    public float getBottom()
    {
        return textBoxes[2].transform.position.y - textBoxes[2].rectTransform.rect.height;
    }
}
