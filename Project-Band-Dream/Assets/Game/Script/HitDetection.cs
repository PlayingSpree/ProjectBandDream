using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection
{
    public List<HitAction> hitActions = new List<HitAction>();
    bool replay = false;
    public void Start(bool replay)
    {
        this.replay = replay;
    }

    public void Update()
    {
        hitActions.Clear();
        if (replay)
        {
            UpdateReplay();
        }
        else
        {
            UpdateTouch();
        }
    }

    void UpdateReplay()
    {
        // Some day...
    }

    void UpdateTouch()
    {
        /*
        Touch[] t = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (t[i].phase == TouchPhase.Began)
            {
                tapLane(t[i].position, false);
            }
            else if (t[i].phase == TouchPhase.Moved)
            {

            }
        }
        */
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            tapLane(Input.mousePosition, false);
        }
    }

    void tapLane(Vector2 pos, bool flick)
    {
        pos = Camera.main.ScreenToWorldPoint(pos);
        if (pos.y <= 0)
        {
            HitAction hitAction = new HitAction(flick);
            float l = (pos.x / GameData.GameSetting.stateSetting.laneWidth) + 3f;
            int mainLane = Mathf.Clamp((int)(l + 0.5f), 0, 6);
            int hitLane = mainLane;
            int checkLane = 0;

            while ((l + mainLane - hitLane <= mainLane + GameData.GameSetting.stateSetting.laneHitExtraSize + 0.5f) && (l + mainLane - hitLane >= mainLane - GameData.GameSetting.stateSetting.laneHitExtraSize - 0.5f))
            {
                if (hitLane >= 0 && hitLane <= 6)
                {
                    hitAction.hitLane.Add(hitLane);
                }
                if (checkLane == 0)
                {
                    if (l % 1f > 0.5f)
                    {
                        hitLane -= ++checkLane;
                    }
                    else
                    {
                        hitLane += ++checkLane;
                    }
                }
                else
                {
                    if (mainLane > hitLane)
                    {
                        hitLane += ++checkLane;
                    }
                    else
                    {
                        hitLane -= ++checkLane;
                    }
                }
            }
            hitActions.Add(hitAction);
        }

    }

    public struct HitAction
    {
        public List<int> hitLane;
        public bool flick;

        public HitAction(bool flick)
        {
            hitLane = new List<int>();
            this.flick = flick;
        }
    }
}
