using System;
using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.VisualEffects
{

    public class TerrainImpactHandler : MonoBehaviour
    {
        private MovementConfig _config;

        private bool _initialised;
        #region Initialisation & Destruction
        
        private void Awake()
        {
            GameController.OnInitialiseUI += Initialise;
        }

        private void Initialise()
        {
            _initialised = true;

            _config = Configuration.Get<MovementConfig>();
            
            MarbleCollisionHandler.OnCollisionGroundGlobal += RegisterImpactGround;
            Player.instance.collisionHandler.OnCollisionMarble += RegisterImpactEnemy;
            MarbleHealth.OnDamageTakenGlobal += RegisterDamage;
        }


        private void OnDestroy()
        {
            GameController.OnInitialiseUI -= Initialise;

            if (_initialised)
            {
                MarbleCollisionHandler.OnCollisionGroundGlobal -= RegisterImpactGround;
                Player.instance.collisionHandler.OnCollisionMarble -= RegisterImpactEnemy;
                MarbleHealth.OnDamageTakenGlobal -= RegisterDamage;
            }            
        }

        #endregion


        private void RegisterImpactGround(Marble marble, Collision collision)
        {
            float magnitude = collision.impulse.magnitude;

            Player.camShake.DoImpactShakeEvent(magnitude, marble.transform.position);

            if (magnitude > _config.minimumVelocityRequiredForImpactDecal)
            {
                CreateImpactDecal(collision, magnitude);            
            }

            AudioPlayData playData = new AudioPlayData()
            {
                clipSet = _config.impactLowClipSet,
                volume = Mathf.Log(magnitude, 2f) * 0.5f,
                worldLocation = marble.transform.position
            };
            AudioEngine.PlaySound(playData);
        }

        private void RegisterImpactEnemy(Collision c, Marble marble)
        {
            float magnitude = c.impulse.magnitude;

            AudioEngine.PlaySound(_config.impactLowClipSet, Mathf.Log(magnitude, 2f) * 0.5f);    
        }

        private void RegisterDamage(MarbleHealth.HealthChangedEvent damageEvent)
        {
            if (damageEvent.damage.doDamageEffects == false)
            {
                return;
            }

            CreateShards(damageEvent);
            Player.camShake.DoDamageShake(damageEvent.totalHealthChange);
        }



        #region Effects:

        private static void CreateShards(MarbleHealth.HealthChangedEvent damageEvent)
        {
            OneShotEffectData effect = new OneShotEffectData(
                            "Marble Hit Shards",
                            damageEvent.marble.transform.position,
                            Quaternion.identity,
                            damageEvent.totalHealthChange);

            VFX.Play(effect);
        }

        private void CreateImpactDecal(Collision c, float force)
        {
            ContactPoint p = c.contacts[0];
            Vector3 position = p.point + (p.normal * 0.09f);
            Quaternion rotation = Quaternion.LookRotation(-p.normal);
            float size = force / 10f;

            OneShotEffectData impactData = new OneShotEffectData("Impact", position, rotation, size);

            VFX.Play(impactData);
        }

        

        #endregion
    }


}

