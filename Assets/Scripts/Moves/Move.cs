using GlobalMoveLists.ActionTypes;
public class Move {
    int manaCost;
    ActionTypes actionType;
    Action actionOnSelect;

    abstract static void invoke() {
        //to be implemented for each subclass
    }
    protected static void setManaCost(int manaCostValue) {
        manaCost = manaCostValue;
    }

    protected static void setActionType (ActionTypes actionType) { 
        actionType = actionType;   
    }

    protected static void setActionOnSelect (Action actionValue) { 
        actionType = actionValue;
    }

}