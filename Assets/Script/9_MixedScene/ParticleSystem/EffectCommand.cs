using CardModel;
using Control;
using System;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
namespace Command
{
    public static class EffectCommand
    {
        public static void AudioEffectPlay(int Rank)
        {
            MainThread.Run(() =>
            {
                AudioSource Source = Info.AudioInfo.Instance.gameObject.AddComponent<AudioSource>();
                Source.clip = Info.AudioInfo.Instance.Clips[Rank];
                Source.Play();
                GameObject.Destroy(Source, Source.clip.length);
            });
        }
        public static void ParticlePlay(int Rank,Card card)
        {
            MainThread.Run(() =>
            {
                ParticleSystem TargetParticle = GameObject.Instantiate(Info.ParticleInfo.Instance.ParticleEffect[Rank]);
                TargetParticle.transform.position = card.transform.position;
                TargetParticle.Play();
                GameObject.Destroy(TargetParticle.gameObject, 2);
            });
        }
        public static void Bullet_Gain(TriggerInfo triggerInfo)
        {
            MainThread.Run(() =>
            {
                GameObject Bullet = GameObject.Instantiate(Info.ParticleInfo.Instance.GainBullet);
                BulletControl bulletControl= Bullet.GetComponent<BulletControl>();
                bulletControl.Init(triggerInfo.triggerCard, triggerInfo.targetCard);
                bulletControl.Play();
                Bullet.GetComponent<ParticleSystem>().Play();
            });
        }
        internal static void Bullet_Hurt(TriggerInfo triggerInfo)
        {
            MainThread.Run(() =>
            {
                GameObject Bullet = GameObject.Instantiate(Info.ParticleInfo.Instance.HurtBullet);
                BulletControl bulletControl = Bullet.GetComponent<BulletControl>();
                bulletControl.Init(triggerInfo.triggerCard, triggerInfo.targetCard);
                bulletControl.Play();
                Bullet.GetComponent<ParticleSystem>().Play();
            });
        }
        public static void TheWorldPlay(Card card)
        {
            MainThread.Run(() =>
            {
                SceneEffectInfo.theWorldEffect.gameObject.SetActive(true);
                Vector3 screenPoint = Camera.main.WorldToScreenPoint(card.transform.position);
                SceneEffectInfo.theWorldEffect.transform.position = Camera.main.ScreenPointToRay(screenPoint).GetPoint(5);
            });
            Task.Run(async () =>
            {
                TheWorldEffect.state = 0;
                await Task.Delay(1000);
                TheWorldEffect.state = 1;
                await Task.Delay(2000);
                TheWorldEffect.state = 0;
                await Task.Delay(1000);
                MainThread.Run(() =>
                {
                    SceneEffectInfo.theWorldEffect.gameObject.SetActive(false);
                });
            });
        }

        
    }
}

