#pragma once

#include "Vector3.hpp"
#include "Interface.hpp"

namespace GTA
{
    public enum class CheckpointIcon
    {
        Traditional = 0,
        SmallArrow = 5,
        DoubleArrow = 6,
        TripleArrow = 7,
        CycleArrow = 8,
        ArrowInCircle = 10,
        DoubleArrowInCircle = 11,
        TripleArrowInCircle = 12,
        CycleArrowInCircle = 13,
        CheckerInCircle = 14,
        Arrow = 15
    };

    public ref class Checkpoint sealed : System::IEquatable<Checkpoint ^>, IHandleable
    {
    public:
        virtual property int Handle
        {
            int get();
        }
        /// <summary>
        /// The icon type of the <see cref="Checkpoint />.
        /// </summary>
        property CheckpointIcon Icon
        {
            CheckpointIcon get();
        }
        /// <summary>
        /// The position of the <see cref="Checkpoint />.
        /// </summary>
        property Math::Vector3 Position
        {
            Math::Vector3 get();
        }
        /// <summary>
        /// The position of the next <see cref="Checkpoint /> the arrow points to.
        /// </summary>
        property Math::Vector3 PointTo
        {
            Math::Vector3 get();
        }
        /// <summary>
        /// The radius of the <see cref="Checkpoint />.
        /// </summary>
        property float Radius
        {
            float get();
        }
        /// <summary>
        /// The color of the <see cref="Checkpoint />.
        /// </summary>
        property System::Drawing::Color Color
        {
            System::Drawing::Color get();
            void set(System::Drawing::Color value);
        }

        void SetCylinderHeight(float nearHeight, float farHeight, float radius);
        void SetIconColor(System::Drawing::Color color);

        void Delete();

        virtual bool Exists();
        static bool Exists(Checkpoint ^checkpoint);

        virtual bool Equals(System::Object ^obj) override;
        virtual bool Equals(Checkpoint ^checkpoint);

        virtual inline int GetHashCode() override
        {
            return Handle;
        }

        static inline bool operator==(Checkpoint ^left, Checkpoint ^right)
        {
            if (ReferenceEquals(left, nullptr))
            {
                return ReferenceEquals(right, nullptr);
            }

            return left->Equals(right);
        }
        static inline bool operator!=(Checkpoint ^left, Checkpoint ^right)
        {
            return !operator==(left, right);
        }

        Checkpoint(CheckpointIcon icon, Math::Vector3 position, Math::Vector3 pointTo, float radius, System::Drawing::Color color);
        ~Checkpoint();
    internal:
        Checkpoint(CheckpointIcon icon, Math::Vector3 position, Math::Vector3 pointTo, float radius, System::Drawing::Color color, int reserved);
    private:
        int _handle = -1;
        CheckpointIcon _icon;
        Math::Vector3 _position;
        Math::Vector3 _pointTo;
        float _radius;
        System::Drawing::Color _color;
        int _reserved;
    };
}