using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace WindowsFormsApp6
{
    public class Particle
    {
        public int Radius;
        public float X;
        public float Y;

        public float Direction;

        public float Speed;

        public float Life;

        

        public static Random rand = new Random();

        public static Particle Generation()
        {
            return new Particle
            {
                Direction = rand.Next(360),
                Speed = 1 + rand.Next(10),
                Radius = rand.Next(10),
                Life = 20 + rand.Next(100),
            };
        }

     

        public virtual void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);

            int alpha = (int)(k * 255);

            var color = Color.FromArgb(alpha, Color.Black);
            var b = new SolidBrush(color);

            
            g.FillEllipse(b, X - Radius, Y - Radius, Radius, Radius);
            

            b.Dispose();
        }
    }



    public class ParticlesColour : Particle
    {
        public Color FromColor;
        public Color ToColor;

        public static Color MixColor(Color color1, Color color2, float k)
        {
            return Color.FromArgb(
                (int)(color2.A * k + color1.A * (1 - k)),
                (int)(color2.R * k + color1.R * (1 - k)),
                (int)(color2.G * k + color1.G * (1 - k)),
                (int)(color2.B * k + color1.B * (1 - k))
            );
        }

        public new static ParticlesColour Generation()
        {
            return new ParticlesColour
            {
                Direction = rand.Next(360),
                Speed = 1 + rand.Next(5),
                Radius = 2 + rand.Next(10),
                Life = 20 + rand.Next(100),
            };
        }
        

        public override void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);

            var color = MixColor(ToColor, FromColor, k);
            var b = new SolidBrush(color);

            
            g.FillEllipse(b, X - Radius, Y - Radius, Radius, Radius);

            b.Dispose();
        }
    }

    public class ParticleImage : Particle
    {
        public Image image = Properties.Resources.rsz_1asteroids;
        public Color FromColor;
        public Color ToColor;

        public new static ParticleImage Generation()
        {
            return new ParticleImage
            {
                Direction = rand.Next(360),
                Speed = 1 + rand.Next(5),
                Radius = 2 + rand.Next(10),
                Life = 20 + rand.Next(100),
            };
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(image, X - Radius, Y - Radius, Radius * 2, Radius * 2);
        }
    }


    public abstract class EmiterBase
    {
        List<Particle> particles = new List<Particle>();

        // количество частиц эмитера храним в переменной
        int particleCount = 0;
        // и отдельной свойство которое возвращает количество частиц
        public int ParticlesCount
        {
            get
            {
                return particleCount;
            }
            set
            {
                // при изменении этого значения
                particleCount = value;
                // удаляем лишние частицы если вдруг
                if (value < particles.Count)
                {
                    particles.RemoveRange(value, particles.Count - value);
                }
            }
        }

        // три абстрактных метода мы их переопределим позже
        public abstract void ResetParticle(Particle particle);
        public abstract void UpdateParticle(Particle particle);
        public abstract Particle CreateParticle();

        // тут общая логика обновления состояния эмитера
        // по сути копипаста
        public void UpdateState()
        {
            foreach (var particle in particles)
            {
                particle.Life -= 5;
                
                if (particle.Life < 0)
                {
                    ResetParticle(particle);
                }
                else
                {
                    UpdateParticle(particle);
                }
            }

            for (var i = 0; i < 10; ++i)
            {
                if (particles.Count < 40)
                {
                    particles.Add(CreateParticle());
                }
                else
                {
                    break;
                }
            }

        }

        public void Render(Graphics g)
        {
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
        }
    }

    public class PointEmiter : EmiterBase
    {
        public Point Position;

        public override Particle CreateParticle()
        {
            var particle = ParticleImage.Generation();
            particle.FromColor = Color.Yellow;
            particle.ToColor = Color.FromArgb(0, Color.Magenta);
            particle.X = 0;
            particle.Y = 0;
            return particle;
        }

        public override void ResetParticle(Particle particle)
        {
            particle.Life = 20 + Particle.rand.Next(30);
            particle.Speed = 1 + Particle.rand.Next(10);
            particle.Direction = Particle.rand.Next(360);
            particle.Radius = 2 + Particle.rand.Next(100);
            particle.X = 0;
            particle.Y = 0;
        }

        public override void UpdateParticle(Particle particle)
        {
            var directionInRadians = particle.Direction / 180 * Math.PI;
            particle.X += (float)(particle.Speed * Math.Cos(directionInRadians));
            particle.Y -= (float)(particle.Speed * Math.Sin(directionInRadians));
        }
    }

    public class DirectionColorfulEmiter : PointEmiter
    {
        public int Direction = 0; // направление частиц
        public int Spread = 10; // разброс частиц
        public Color FromColor = Color.White; // исходный цвет
        public Color ToColor = Color.White; // конечный цвет

        public override Particle CreateParticle()
        {
            var particle = ParticleImage.Generation();
            particle.FromColor = this.FromColor;
            particle.ToColor = Color.FromArgb(0, this.ToColor);
            particle.Direction = this.Direction + Particle.rand.Next(-Spread / 2, Spread / 2);
            particle.Speed = 1 + Particle.rand.Next(30);

           
            return particle;
        }

        public override void ResetParticle(Particle particle)
        {
            var particleColorful = particle as ParticleImage;
            if (particleColorful != null)
            {
                particleColorful.Life = 20 + Particle.rand.Next(30000);
                particleColorful.Speed = 1 + Particle.rand.Next(1);

                particleColorful.FromColor = this.FromColor;
                particleColorful.ToColor = Color.FromArgb(0, this.ToColor);
                particleColorful.Direction = this.Direction + Particle.rand.Next(-Spread / 2, Spread / 2);

                particleColorful.X += Position.X/50;
                particleColorful.Y += Position.Y + 10 ;
            }
        }
    }

    

}



